﻿// Copyright 2019 Google LLC
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

// Generated code. DO NOT EDIT!

namespace Testing.ResourceNames.Snippets
{
    using System.Collections.Generic;
    using Testing.ResourceNames;

    public sealed partial class GeneratedResourceNamesClientStandaloneSnippets
    {
        /// <summary>Snippet for WildcardMultiPatternMethod</summary>
        public void WildcardMultiPatternMethodResourceNames1()
        {
            // Snippet: WildcardMultiPatternMethod(WildcardMultiPatternName, WildcardMultiPatternName, IEnumerable<WildcardMultiPatternName>, CallSettings)
            // Create client
            ResourceNamesClient resourceNamesClient = ResourceNamesClient.Create();
            // Initialize request argument(s)
            WildcardMultiPatternName name = WildcardMultiPatternName.FromSingularItem();
            WildcardMultiPatternName @ref = WildcardMultiPatternName.FromSingularItem();
            IEnumerable<WildcardMultiPatternName> repeatedRef = new WildcardMultiPatternName[]
            {
                WildcardMultiPatternName.FromSingularItem(),
            };
            // Make the request
            Response response = resourceNamesClient.WildcardMultiPatternMethod(name, @ref, repeatedRef);
            // End snippet
        }
    }
}