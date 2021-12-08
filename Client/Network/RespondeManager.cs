using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using CNetwork;

namespace UI.Network {
	public class RespondeManager {

		private readonly Dictionary<Type, ConcurrentQueue<TaskCompletionSource<IPacket>>> responseTasks = new Dictionary<Type, ConcurrentQueue<TaskCompletionSource<IPacket>>>();

		private ConcurrentQueue<TaskCompletionSource<IPacket>> getQueue(Type type) {
			ConcurrentQueue<TaskCompletionSource<IPacket>> result;
			bool contain = responseTasks.TryGetValue(type, out result);
			if (!contain) {
				lock (responseTasks) {
					contain = responseTasks.TryGetValue(type, out result);
					if (contain)
						return result;
					result = new ConcurrentQueue<TaskCompletionSource<IPacket>>();
					responseTasks.Add(type, result);
				}
			}
			return result;
		}

		public Task<IPacket> CreateRespondeWaiter<TResponde>(IPacket outbound) where TResponde : IPacket
		{
			TaskCompletionSource<IPacket> tcs = new TaskCompletionSource<IPacket>();
			getQueue(typeof(TResponde)).Enqueue(tcs);
			return tcs.Task;
		}

		public bool RespondToResponse(IPacket responde) {
			Type respondeType = responde.GetType();
			TaskCompletionSource<IPacket> tcs;
			if (getQueue(respondeType).TryDequeue(out tcs)) {
				tcs.TrySetResult(responde);
				return true;
			}
			return false;
		}
		
	}
}