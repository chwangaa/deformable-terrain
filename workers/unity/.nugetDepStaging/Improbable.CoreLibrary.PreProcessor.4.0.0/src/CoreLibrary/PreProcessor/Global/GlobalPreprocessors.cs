using System;
using System.Collections.Generic;
using Improbable.Corelib.PreProcessors;

namespace Improbable.Corelibrary.PreProcessor.Global
{
    /// <summary>
    ///     Class which defines a set of Global Pre-Processors which can by applied at load or compile time.
    /// </summary>
    public static class GlobalPreProcessors
    {
        public static HashSet<Type> Preprocessors = new HashSet<Type> { typeof(PrefabComponentRemovingPreprocessor) };
    }
}