﻿  //
// TextEditorDataTests.cs
//
// Author:
//       mkrueger <>
//
// Copyright (c) 2017 ${CopyrightHolder}
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
using Mono.TextEditor;
using NUnit.Framework;
using System.Linq;
using MonoDevelop.Core.Text;

namespace Mono.TextEditor.Tests
{
	[TestFixture]
	public class TextEditorDataTests
	{
		/// <summary>
		/// Bug 53283 - Autocompletion doesn't respect tabs versus spaces
		/// </summary>
		[Test]
		public void TestBug53283 ()
		{
			var data = new TextEditorData ();
			data.Options.TabsToSpaces = false;
			data.Options.TabSize = 4;
			data.Insert (0, "\t");
			Assert.AreEqual ("\t", data.Text);
			data.Options.TabsToSpaces = true;
			data.Options.TabSize = 4;
			data.Replace (0, data.Length, "\t");
			Assert.AreEqual ("    ", data.Text);
		}
 	}
}
