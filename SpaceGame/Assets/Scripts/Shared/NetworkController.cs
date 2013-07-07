//
// This code file was taken from the thread here 
// http://forum.unity3d.com/threads/125084-objects-owned-by-other-players-twitch-when-moving-in-the-same-direction-as-the-player
//

using UnityEngine;
using System.Collections;

public class NetworkController : MonoBehaviour
{

	public double m_InterpolationBackTime = 0.1;
	public double m_ExtrapolationLimit = 0.5;
	public float InterpolationConstant = 0.1f;

	internal struct State
	{
		internal double timestamp;
		internal Vector3 pos;
		internal Vector3 velocity;
		internal Quaternion rot;
		internal Vector3 angularVelocity;
	}

	// We store twenty states with "playback" information
	State[] m_BufferedState = new State[20];
	// Keep track of what slots are used
	int m_TimestampCount;

	void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
	{
		// Send data to server
		if (stream.isWriting)
		{
			Vector3 pos = transform.position;
			Quaternion rot = transform.rotation;
			Vector3 velocity = rigidbody.velocity;
			Vector3 angularVelocity = rigidbody.angularVelocity;

			stream.Serialize(ref pos);
			stream.Serialize(ref velocity);
			stream.Serialize(ref rot);
			stream.Serialize(ref angularVelocity);
		}
		// Read data from remote client
		else
		{
			Vector3 pos = Vector3.zero;
			Vector3 velocity = Vector3.zero;
			Quaternion rot = Quaternion.identity;
			Vector3 angularVelocity = Vector3.zero;

			stream.Serialize(ref pos);
			stream.Serialize(ref velocity);
			stream.Serialize(ref rot);
			stream.Serialize(ref angularVelocity);

			// Shift the buffer sideways, deleting state 20
			for (int i = m_BufferedState.Length - 1; i >= 1; i--)
			{
				m_BufferedState[i] = m_BufferedState[i - 1];
			}

			// Record current state in slot 0
			State state;
			state.timestamp = info.timestamp;
			state.pos = pos;
			state.velocity = velocity;
			state.rot = rot;
			state.angularVelocity = angularVelocity;

			m_BufferedState[0] = state;

			Debug.Log("State " + info.timestamp + " " + pos + " " + velocity);

			// Update used slot count, however never exceed the buffer size
			// Slots aren't actually freed so this just makes sure the buffer is
			// filled up and that uninitalized slots aren't used.
			m_TimestampCount = Mathf.Min(m_TimestampCount + 1, m_BufferedState.Length);

			// Check if states are in order, if it is inconsistent you could reshuffel or 
			// drop the out-of-order state. Nothing is done here
			for (int i = 0; i < m_TimestampCount - 1; i++)
			{
				if (m_BufferedState[i].timestamp < m_BufferedState[i + 1].timestamp)
					Debug.Log("State inconsistent");
			}
		}
	}

	// We have a window of interpolationBackTime where we basically play 
	// By having interpolationBackTime the average ping, you will usually use interpolation.
	// And only if no more data arrives we will use extra polation
	void FixedUpdate()
	{
		// This is the target playback time of the rigid body
		double interpolationTime = Network.time - m_InterpolationBackTime;

		// Use interpolation if the target playback time is present in the buffer
		if (m_BufferedState[0].timestamp > interpolationTime)
		{
			// Go through buffer and find correct state to play back
			for (int i = 1; i < m_TimestampCount; i++)
			{
				if (m_BufferedState[i].timestamp <= interpolationTime || i == m_TimestampCount - 1)
				{
					State rhs = m_BufferedState[i - 1];
					// The best playback state (closest to 100 ms old (default time))
					State lhs = m_BufferedState[i];

					Vector3 y0;
					Vector3 y1 = lhs.pos;
					Vector3 y2 = rhs.pos;
					Vector3 y3;

					Vector3 v0;
					Vector3 v1 = lhs.velocity;
					Vector3 v2 = rhs.velocity;
					Vector3 v3;

					float length = (float)(rhs.timestamp - lhs.timestamp);
					if (i == m_TimestampCount - 1)
					{
						v0 = lhs.velocity;
						y0 = lhs.pos - v0 * length;
					}
					else
					{
						v0 = m_BufferedState[i + 1].velocity;
						y0 = m_BufferedState[i + 1].pos;
					}

					if (i == 1)
					{
						v3 = rhs.velocity;
						y3 = rhs.pos + rhs.velocity * length;
					}
					else
					{
						v3 = m_BufferedState[i - 2].velocity;
						y3 = m_BufferedState[i - 2].pos;
					}

					// Use the time between the two slots to determine if interpolation is necessary
					float t = 0.0F;
					// As the time difference gets closer to 100 ms t gets closer to 1 in 
					// which case rhs is only used
					// Example:
					// Time is 10.000, so sampleTime is 9.900 
					// lhs.time is 9.910 rhs.time is 9.980 length is 0.070
					// t is 9.900 - 9.910 / 0.070 = 0.14. So it uses 14% of rhs, 86% of lhs
					if (length > 0.0001)
					{
						t = (float)((interpolationTime - lhs.timestamp) / length);
					}
					//  Debug.Log(t);
					// if t=0 => lhs is used directly
					transform.localPosition = Vector3.Lerp(transform.localPosition,
										Vector3.Lerp(lhs.pos, rhs.pos, t),
										InterpolationConstant);

					transform.localRotation = Quaternion.Slerp(transform.localRotation,
																Quaternion.Slerp(lhs.rot, rhs.rot, t),
																InterpolationConstant);

					rigidbody.velocity = Vector3.Lerp(rigidbody.velocity,
													   Vector3.Lerp(lhs.velocity, rhs.velocity, t),
													   InterpolationConstant);

					rigidbody.angularVelocity = Vector3.Lerp(rigidbody.angularVelocity,
															  Vector3.Lerp(lhs.angularVelocity, rhs.angularVelocity, t),
															  InterpolationConstant);
					return;
				}
			}
		}
	}

	Vector3 Vector3CubicInterpolate(
		Vector3 y0, Vector3 y1,
		Vector3 y2, Vector3 y3,
		float mu)
	{
		Vector3 a0, a1, a2, a3;
		float mu2;

		mu2 = mu * mu;
		a0 = -0.5f * y0 + 1.5f * y1 - 1.5f * y2 + 0.5f * y3;
		a1 = y0 - 2.5f * y1 + 2 * y2 - 0.5f * y3;
		a2 = -0.5f * y0 + 0.5f * y2;
		a3 = y1;

		// a0 = y3 - y2 - y0 + y1;
		// a1 = y0 - y1 - a0;
		// a2 = y2 - y0;
		// a3 = y1;

		return (a0 * mu * mu2 + a1 * mu2 + a2 * mu + a3);
	}
}
