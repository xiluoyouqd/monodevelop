//
// RoslynTypeSystemService.cs
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
using Microsoft.CodeAnalysis;
using System.Linq;
using System.IO;
using MonoDevelop.Core;
using System.Collections.Generic;
using System.Threading;
using System.Reflection;
using MonoDevelop.Ide.Editor;
using MonoDevelop.Core.Text;
using Microsoft.CodeAnalysis.Text;

namespace MonoDevelop.Ide.TypeSystem
{
	sealed class MonoDevelopSourceTextContainer : SourceTextContainer, IDisposable
	{
		readonly ITextDocument document;
		bool isDisposed;
		MonoDevelopSourceText currentText;

		public DocumentId Id {
			get;
			private set;
		}

		public MonoDevelopSourceTextContainer (DocumentId documentId, ITextDocument document) : this (document)
		{
			Id = documentId;
		}

		public MonoDevelopSourceTextContainer (ITextDocument document)
		{
			this.document = document;
			this.document.TextChanging += HandleTextReplacing;
			this.document.TextChanged += Document_TextChanged;
		}

		void HandleTextReplacing (object sender, Core.Text.TextChangeEventArgs e)
		{
			var handler = TextChanged;
			if (handler != null) {
				var oldText = CurrentText;
				var newText = oldText.Replace (e.Offset, e.RemovalLength, e.InsertedText.Text);
				handler (this, new Microsoft.CodeAnalysis.Text.TextChangeEventArgs (oldText, newText, new TextChangeRange(TextSpan.FromBounds (e.Offset, e.Offset + e.RemovalLength), e.InsertionLength)));
			}
		}

		void Document_TextChanged (object sender, Core.Text.TextChangeEventArgs e)
		{
			currentText = null;
		}

		public void Dispose ()
		{
			if (isDisposed)
				return;
			currentText = null;
			document.TextChanging -= HandleTextReplacing;
			document.TextChanged -= Document_TextChanged;;
			isDisposed = true;
		}

		#region implemented abstract members of SourceTextContainer
		public override SourceText CurrentText {
			get {
				if (currentText == null) {
					currentText = new MonoDevelopSourceText (document.CreateDocumentSnapshot ());
				}
				return currentText;
			}
		}

		public ITextDocument Document {
			get {
				return document;
			}
		}

		public override event EventHandler<Microsoft.CodeAnalysis.Text.TextChangeEventArgs> TextChanged;
		#endregion
	}
}