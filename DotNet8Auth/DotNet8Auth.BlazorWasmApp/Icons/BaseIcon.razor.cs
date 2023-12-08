// ------------------------------------------------------------
// Copyright (c) 2022, Brian Parker All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace DotNet8Auth.BlazorWasmApp.Icons
{
    public abstract partial class BaseIcon
    {
        private string path;

        protected BaseIcon(string source) => path = $"images/bootstrap-icons/bootstrap-icons.svg#{source}";

        [Parameter]
        public int Size { get; set; } = 24;

        [Parameter(CaptureUnmatchedValues = true)]
        public Dictionary<string, object> UncapturedAttributes { get; set; }
    }
}