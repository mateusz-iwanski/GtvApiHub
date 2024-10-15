using System;

/// <summary>
/// <c>DeserializeWebApiResponseAttribute</c> is used to deserialize the response from the Web Api server
/// </summary>
/// <remarks>
/// It works with the DeserializeWebApiResponseAsync.
/// </remarks>
[AttributeUsage(AttributeTargets.Method, Inherited = true)]
public class DeserializeWebApiResponseAttribute : Attribute
{
}