# ChineseNumerals.ToInteger method (1 of 2)

Gets the numeric value for any single Chinese character.

```csharp
public static int? ToInteger(char ch)
```

| parameter | description |
| --- | --- |
| ch | The Chinese character. |

## Return Value

The numeric value, or `null` if the value is not a Chinese character, or is simply not supported.

## See Also

* class [ChineseNumerals](../ChineseNumerals.md)
* namespace [Faithlife.Globalization.Numerals](../../Faithlife.Globalization.md)

---

# ChineseNumerals.ToInteger method (2 of 2)

Converts the Chinese numeral string to an integer.

```csharp
public static int? ToInteger(string chineseNumericString)
```

| parameter | description |
| --- | --- |
| chineseNumericString | The Chinese numeral string. |

## Return Value

An integer with the value represented by the Chinese numeral, or `null` if the string was in a bad format, contained non Chinese numerals, or contained Chinese numerals that aren't supported.

## See Also

* class [ChineseNumerals](../ChineseNumerals.md)
* namespace [Faithlife.Globalization.Numerals](../../Faithlife.Globalization.md)

<!-- DO NOT EDIT: generated by xmldocmd for Faithlife.Globalization.dll -->