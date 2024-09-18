// Copyright 2019 Google LLC
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

namespace GoogleCSharpSnippets
{
    using System.Threading.Tasks;
    using Testing.PublishingSettings;

    /// <summary>Generated snippets.</summary>
    public sealed class AllGeneratedServiceWithSelectiveGenerationClientSnippets
    {
        /// <summary>Snippet for PublicMethod</summary>
        public void PublicMethodRequestObject()
        {
            // Snippet: PublicMethod(Request, CallSettings)
            // Create client
            ServiceWithSelectiveGenerationClient serviceWithSelectiveGenerationClient = ServiceWithSelectiveGenerationClient.Create();
            // Initialize request argument(s)
            Request request = new Request
            {
                String1 = "",
                String2 = "",
                RequestId = "",
                RequestIdWithPresence = "",
            };
            // Make the request
            Response response = serviceWithSelectiveGenerationClient.PublicMethod(request);
            // End snippet
        }

        /// <summary>Snippet for PublicMethodAsync</summary>
        public async Task PublicMethodRequestObjectAsync()
        {
            // Snippet: PublicMethodAsync(Request, CallSettings)
            // Additional: PublicMethodAsync(Request, CancellationToken)
            // Create client
            ServiceWithSelectiveGenerationClient serviceWithSelectiveGenerationClient = await ServiceWithSelectiveGenerationClient.CreateAsync();
            // Initialize request argument(s)
            Request request = new Request
            {
                String1 = "",
                String2 = "",
                RequestId = "",
                RequestIdWithPresence = "",
            };
            // Make the request
            Response response = await serviceWithSelectiveGenerationClient.PublicMethodAsync(request);
            // End snippet
        }

        /// <summary>Snippet for SelectiveMethod</summary>
        public void SelectiveMethodRequestObject()
        {
            // Snippet: SelectiveMethod(Request, CallSettings)
            // Create client
            ServiceWithSelectiveGenerationClient serviceWithSelectiveGenerationClient = ServiceWithSelectiveGenerationClient.Create();
            // Initialize request argument(s)
            Request request = new Request
            {
                String1 = "",
                String2 = "",
                RequestId = "",
                RequestIdWithPresence = "",
            };
            // Make the request
            Response response = serviceWithSelectiveGenerationClient.SelectiveMethod(request);
            // End snippet
        }

        /// <summary>Snippet for SelectiveMethodAsync</summary>
        public async Task SelectiveMethodRequestObjectAsync()
        {
            // Snippet: SelectiveMethodAsync(Request, CallSettings)
            // Additional: SelectiveMethodAsync(Request, CancellationToken)
            // Create client
            ServiceWithSelectiveGenerationClient serviceWithSelectiveGenerationClient = await ServiceWithSelectiveGenerationClient.CreateAsync();
            // Initialize request argument(s)
            Request request = new Request
            {
                String1 = "",
                String2 = "",
                RequestId = "",
                RequestIdWithPresence = "",
            };
            // Make the request
            Response response = await serviceWithSelectiveGenerationClient.SelectiveMethodAsync(request);
            // End snippet
        }
    }
}
