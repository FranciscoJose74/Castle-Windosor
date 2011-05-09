#region license

// Copyright 2004-2011 Castle Project - http://www.castleproject.org/
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

#endregion

using System;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using Castle.Services.Transaction.Internal;
using log4net;

namespace Castle.Services.Transaction.IO
{
	/// <summary>
	/// 	Adapter which wraps the functionality in <see cref = "File" />
	/// 	together with native kernel transactions.
	/// </summary>
	public class DirectoryAdapter : TransactionAdapterBase, IDirectoryAdapter
	{
		private readonly IMapPath _PathFinder;
		private readonly ILog _Logger = LogManager.GetLogger(typeof (DirectoryAdapter));

		/// <summary>
		/// 	Create a new DirectoryAdapter instance. C'tor.
		/// </summary>
		/// <param name = "pathFinder">The MapPath implementation.</param>
		/// <param name = "constrainToSpecifiedDir">Whether to ChJail the DirectoryAdapter.</param>
		/// <param name = "specifiedDir">The directory to constrain the adapter to.</param>
		public DirectoryAdapter(IMapPath pathFinder, bool constrainToSpecifiedDir, string specifiedDir)
			: base(constrainToSpecifiedDir, specifiedDir)
		{
			Contract.Requires(pathFinder != null);

			_Logger.Debug("DirectoryAdapter created.");

			_PathFinder = pathFinder;
		}

		bool IDirectoryAdapter.Create(string path)
		{
			AssertAllowed(path);

#if !MONO
			ITransaction tx;
			if (HasTransaction(out tx))
			{
				return ((IDirectoryAdapter) tx).Create(path);
			}
#endif
			if (((IDirectoryAdapter) this).Exists(path))
			{
				return true;
			}

			LongPathDirectory.Create(path);
			return true;
		}

		bool IDirectoryAdapter.Exists(string path)
		{
			AssertAllowed(path);
#if !MONO
			ITransaction tx;
			if (HasTransaction(out tx))
				return ((IDirectoryAdapter) tx).Exists(path);
#endif

			return LongPathDirectory.Exists(path);
		}

		void IDirectoryAdapter.Delete(string path)
		{
			AssertAllowed(path);
#if !MONO
			ITransaction tx;
			if (HasTransaction(out tx))
			{
				((IDirectoryAdapter) tx).Delete(path);
				return;
			}
#endif

			LongPathDirectory.Delete(path);
		}

		bool IDirectoryAdapter.Delete(string path, bool recursively)
		{
			AssertAllowed(path);
#if !MONO
			ITransaction tx;
			if (HasTransaction(out tx))
			{
				return ((IDirectoryAdapter) tx).Delete(path, recursively);
			}
#endif

			LongPathDirectory.Delete(path);
			return true;
		}

		string IDirectoryAdapter.GetFullPath(string path)
		{
			AssertAllowed(path);
#if !MONO
			ITransaction tx;
			if (HasTransaction(out tx))
				return ((IDirectoryAdapter) tx).GetFullPath(path);
#endif
			return LongPathCommon.NormalizeLongPath(path);
		}

		string IDirectoryAdapter.MapPath(string path)
		{
			return _PathFinder.MapPath(path);
		}

		void IDirectoryAdapter.Move(string originalPath, string newPath)
		{
			AssertAllowed(originalPath);
			AssertAllowed(newPath);

#if !MONO
			ITransaction tx;
			if (HasTransaction(out tx))
			{
				((IDirectoryAdapter) this).Move(originalPath, newPath);
				return;
			}
#endif

			LongPathFile.Move(originalPath, newPath);
		}
	}
}