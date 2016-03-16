#region Apache License
//
// Licensed to the Apache Software Foundation (ASF) under one or more 
// contributor license agreements. See the NOTICE file distributed with
// this work for additional information regarding copyright ownership. 
// The ASF licenses this file to you under the Apache License, Version 2.0
// (the "License"); you may not use this file except in compliance with 
// the License. You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
#endregion

using System;
using System.Globalization;
using log4net.Appender;
using log4net.Layout;
using log4net.Core;

namespace Improbable.Unity.Logging
{
	public class UnityConsoleAppender : AppenderSkeleton
	{
		#region Public Instance Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="ConsoleAppender" /> class.
		/// </summary>
		/// <remarks>
		/// The instance of the <see cref="ConsoleAppender" /> class is set up to write 
		/// to the standard output stream.
		/// </remarks>
		public UnityConsoleAppender() 
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ConsoleAppender" /> class
		/// with the specified layout.
		/// </summary>
		/// <param name="layout">the layout to use for this appender</param>
		/// <remarks>
		/// The instance of the <see cref="ConsoleAppender" /> class is set up to write 
		/// to the standard output stream.
		/// </remarks>
		[Obsolete("Instead use the default constructor and set the Layout property")]
		public UnityConsoleAppender(ILayout layout) : this(layout, false)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ConsoleAppender" /> class
		/// with the specified layout.
		/// </summary>
		/// <param name="layout">the layout to use for this appender</param>
		/// <param name="writeToErrorStream">flag set to <c>true</c> to write to the console error stream</param>
		/// <remarks>
		/// When <paramref name="writeToErrorStream" /> is set to <c>true</c>, output is written to
		/// the standard error output stream.  Otherwise, output is written to the standard
		/// output stream.
		/// </remarks>
		[Obsolete("Instead use the default constructor and set the Layout & Target properties")]
		public UnityConsoleAppender(ILayout layout, bool writeToErrorStream) 
		{
			Layout = layout;
			m_writeToErrorStream = writeToErrorStream;
		}

		#endregion Public Instance Constructors

		#region Public Instance Properties

		/// <summary>
		/// Target is the value of the console output stream.
		/// This is either <c>"Console.Out"</c> or <c>"Console.Error"</c>.
		/// </summary>
		/// <value>
		/// Target is the value of the console output stream.
		/// This is either <c>"Console.Out"</c> or <c>"Console.Error"</c>.
		/// </value>
		/// <remarks>
		/// <para>
		/// Target is the value of the console output stream.
		/// This is either <c>"Console.Out"</c> or <c>"Console.Error"</c>.
		/// </para>
		/// </remarks>
		virtual public string Target
		{
			get { return m_writeToErrorStream ? ConsoleError : ConsoleOut; }
			set
			{
				string v = value.Trim();
				
				if (string.Compare(ConsoleError, v, true, CultureInfo.InvariantCulture) == 0) 
				{
					m_writeToErrorStream = true;
				} 
				else 
				{
					m_writeToErrorStream = false;
				}
			}
		}

		#endregion Public Instance Properties

		#region Override implementation of AppenderSkeleton

		/// <summary>
		/// This method is called by the <see cref="AppenderSkeleton.DoAppend(LoggingEvent)"/> method.
		/// </summary>
		/// <param name="loggingEvent">The event to log.</param>
		/// <remarks>
		/// <para>
		/// Writes the event to the console.
		/// </para>
		/// <para>
		/// The format of the output will depend on the appender's layout.
		/// </para>
		/// </remarks>
		override protected void Append(LoggingEvent loggingEvent) 
		{
#if NETCF_1_0
			// Write to the output stream
			Console.Write(RenderLoggingEvent(loggingEvent));
#else
			if (m_writeToErrorStream)
			{
				// Write to the error stream
				UnityEngine.Debug.LogError(RenderLoggingEvent(loggingEvent));
			}
			else
			{
				// Write to the output stream
			    if (loggingEvent.Level >= Level.Error)
				    UnityEngine.Debug.LogError(RenderLoggingEvent(loggingEvent));
                else if (loggingEvent.Level >= Level.Warn)
                    UnityEngine.Debug.LogWarning(RenderLoggingEvent(loggingEvent));
                else
                    UnityEngine.Debug.Log(RenderLoggingEvent(loggingEvent));
			}
#endif
		}

		/// <summary>
		/// This appender requires a <see cref="log4net.Layout"/> to be set.
		/// </summary>
		/// <value><c>true</c></value>
		/// <remarks>
		/// <para>
		/// This appender requires a <see cref="log4net.Layout"/> to be set.
		/// </para>
		/// </remarks>
		override protected bool RequiresLayout
		{
			get { return true; }
		}

		#endregion Override implementation of AppenderSkeleton

		#region Public Static Fields

		/// <summary>
		/// The <see cref="ConsoleAppender.Target"/> to use when writing to the Console 
		/// standard output stream.
		/// </summary>
		/// <remarks>
		/// <para>
		/// The <see cref="ConsoleAppender.Target"/> to use when writing to the Console 
		/// standard output stream.
		/// </para>
		/// </remarks>
		public const string ConsoleOut = "Console.Out";

		/// <summary>
		/// The <see cref="ConsoleAppender.Target"/> to use when writing to the Console 
		/// standard error output stream.
		/// </summary>
		/// <remarks>
		/// <para>
		/// The <see cref="ConsoleAppender.Target"/> to use when writing to the Console 
		/// standard error output stream.
		/// </para>
		/// </remarks>
		public const string ConsoleError = "Console.Error";

		#endregion Public Static Fields

		#region Private Instances Fields

		private bool m_writeToErrorStream = false;

		#endregion Private Instances Fields
	}
}
