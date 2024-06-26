﻿// Copyright 2020 Google LLC
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     https://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using Google.Api.Generator.Utils;
using Google.Api.Generator.Utils.Roslyn;
using Google.Apis.Util;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Google.Api.Generator.Utils.Roslyn.RoslynBuilder;

namespace Google.Api.Generator.Rest.Models
{
    /// <summary>
    /// Model for a member of <see cref="EnumModel"/>.
    /// </summary>
    public class EnumMemberModel
    {
        /// <summary>
        /// The name of the member in C#, within the enum.
        /// </summary>
        private string MemberName { get; }

        /// <summary>
        /// The original string value as specified in the Discovery doc.
        /// </summary>
        private string OriginalValue { get; }

        /// <summary>
        /// The description of the value (if any) for use in documentation.
        /// </summary>
        private string Description { get; }

        /// <summary>
        /// The integer value of the enum.
        /// </summary>
        private int NumericValue { get; }

        public EnumMemberModel(string textValue, string description, int numericValue)
        {
            OriginalValue = textValue;
            MemberName = textValue.ToMemberName();
            Description = description;
            NumericValue = numericValue;
        }

        public EnumMemberDeclarationSyntax GenerateDeclaration(SourceFileContext ctx) =>
            EnumMember(MemberName, NumericValue)
                .WithAttribute(ctx.Type<StringValueAttribute>())(OriginalValue)
                .MaybeWithXmlDoc(XmlDoc.MaybeSummary(Description));
    }
}
