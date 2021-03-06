﻿using System;

namespace Signals
{
	public sealed class Slot<T1, T2, T3>:Slot
	{
		internal Slot()
		{

		}

		public new Signal<T1, T2, T3> Signal
		{
			get
			{
				return (Signal<T1, T2, T3>)signal;
			}
		}

		public new Action<T1, T2, T3> Listener
		{
			get
			{
				return (Action<T1, T2, T3>)listener;
			}
		}
	}
}