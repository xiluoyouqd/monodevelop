﻿//
// DocumentContext.cs
//
// Author:
//       Mike Krüger <mkrueger@xamarin.com>
//
// Copyright (c) 2014 Xamarin Inc. (http://xamarin.com)
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
using System;
using MonoDevelop.Projects;
using ICSharpCode.NRefactory.TypeSystem;
using MonoDevelop.Ide.TypeSystem;

namespace MonoDevelop.Ide.Editor
{
	/// <summary>
	/// A document context puts a textual document in a semantic context inside a project and gives access
	/// to the parse information of the textual document.
	/// </summary>
	public abstract class DocumentContext : ICSharpCode.NRefactory.AbstractAnnotatable
	{
		/// <summary>
		/// The name of the document. It's the file name for files on disc. 
		/// For unsaved files that name is different.
		/// </summary>
		public abstract string Name {
			get;
		}

		/// <summary>
		/// Project != null
		/// </summary>
		public virtual bool HasProject {
			get { return Project != null; }
		}

		/// <summary>
		/// Gets the project this context is in.
		/// </summary>
		public abstract Project Project {
			get;
		}


		/// <summary>
		/// Returns the roslyn document for this document. This may return <c>null</c> if it's no compileable document.
		/// Even if it's a C# file.
		/// </summary>
		public abstract Microsoft.CodeAnalysis.Document AnalysisDocument {
			get;
		}
		
		/// <summary>
		/// The parsed document. Contains all syntax information about the text.
		/// </summary>
		public abstract ParsedDocument ParsedDocument {
			get;
		}

		/// <summary>
		/// If true, then ProjectContent and Compilation are invalid/outdated.
		/// </summary>
		public virtual bool IsProjectContextInUpdate {
			get {
				return false;
			}
		}

		/// <summary>
		/// If true, the document is part of the ProjectContent.
		/// </summary>
		public virtual bool IsCompileableInProject {
			get {
				return true;
			}
		}

		public virtual T GetContent<T> () where T : class
		{
			var t = this as T;
			if (t != null)
				return t;
			return null;
		}

		/// <summary>
		/// This is called after the ParsedDocument updated.
		/// </summary>
		public event EventHandler DocumentParsed;

		protected void OnDocumentParsed (EventArgs e)
		{
			var handler = DocumentParsed;
			if (handler != null)
				handler (this, e);
		}

		public abstract void AttachToProject (Project project);

		/// <summary>
		/// Forces a reparse of the document. This call doesn't block the ui thread. 
		/// The next call to ParsedDocument will give always the current parsed document but may block the UI thread.
		/// </summary>
		public abstract void ReparseDocument ();

		// TODO: IMO that needs to be handled differently (this is atm only used in the ASP.NET binding)
		// Maybe using the file service. Files can be changed/saved w/o beeing opened.
		public event EventHandler Saved;

		protected virtual void OnSaved (EventArgs e)
		{
			var handler = Saved;
			if (handler != null)
				handler (this, e);
		}
	}
}