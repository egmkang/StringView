A `ReadOnly` `StringSlice` or `string_view` in sharp.
because `System.String.Substring` will allocate new spaces and copy the chars, so `StringView` won't do this.
`StringView` only contains String's reference and the slice's begin and length property,
and many algorithm based on these three property.

there is not one complete benchmark between `String` and `StringView`, i will finish this in few days.

StringView supply:
* IndexOf
* IndexOfAny
* LastIndexOf
* LastIndexOfAny
* Contains
* StartsWith
* EndsWith
* Substring
* Split
* Concat
* Join