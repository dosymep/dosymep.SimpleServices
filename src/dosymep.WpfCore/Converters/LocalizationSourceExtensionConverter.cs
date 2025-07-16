// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

//
//
//
//  Contents:  Implements a converter to an instance descriptor for 
//             DynamicResourceExtension
//
//

using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Windows;

namespace dosymep.WpfCore.Converters;

/// <summary>
/// Type converter to inform the serialization system how to construct a DynamicResourceExtension from
/// an instance. It reports that ResourceKey should be used as the first parameter to the constructor.
/// https://github.com/dotnet/wpf/blob/main/src/Microsoft.DotNet.Wpf/src/PresentationFramework/System/Windows/DynamicResourceExtensionConverter.cs
/// </summary>
internal sealed class LocalizationSourceExtensionConverter : TypeConverter {
    /// <summary>
    /// True if converting to an instance descriptor
    /// </summary>
    public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType) {
        return destinationType == typeof(InstanceDescriptor) || base.CanConvertTo(context, destinationType);
    }

    /// <summary>
    /// Converts to an instance descriptor
    /// </summary>
    public override object? ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture,
        object value, Type destinationType) {
        if(destinationType != typeof(InstanceDescriptor)) {
            return base.ConvertTo(context, culture, value, destinationType);
        }

        if(value == null) {
            throw new ArgumentNullException(nameof(value));
        }

        if(value is not DynamicResourceExtension dynamicResource)
            throw new ArgumentException("'value' must be of the type 'DynamicResourceExtension'", nameof(value));

        return new InstanceDescriptor(
            typeof(DynamicResourceExtension).GetConstructor([typeof(object)]), new[] {dynamicResource.ResourceKey});
    }
}