namespace WrappedSTJson

open System.Text.Json

module JsonElm =
    let isUndefined: JsonElement -> bool = _.ValueKind >> (=) JsonValueKind.Undefined
    let isObject: JsonElement -> bool = _.ValueKind >> (=) JsonValueKind.Object
    let isArray: JsonElement -> bool = _.ValueKind >> (=) JsonValueKind.Array
    let isString: JsonElement -> bool = _.ValueKind >> (=) JsonValueKind.String
    let isNumber: JsonElement -> bool = _.ValueKind >> (=) JsonValueKind.Number
    let isNull: JsonElement -> bool = _.ValueKind >> (=) JsonValueKind.Null

    let isBool: JsonElement -> bool =
        _.ValueKind >> fun x -> x = JsonValueKind.True || x = JsonValueKind.False

    let private pureIf: (JsonElement -> bool) -> JsonElement -> JsonElement option =
        fun isValue -> Some >> Option.filter isValue

    let private getRefValue: (JsonElement -> bool) -> (JsonElement -> bool * 'value) -> JsonElement -> 'value option =
        fun isValue getTryValue -> pureIf isValue >> Option.map getTryValue >> Option.filter fst >> Option.map snd

    let private getRefNullableValue
        : (JsonElement -> bool) -> (JsonElement -> bool * 'value | null) -> JsonElement -> 'value option =
        fun isValue getTryValue ->
            pureIf isValue
            >> Option.map getTryValue
            >> Option.filter fst
            >> Option.bind (snd >> NullOpt.toOption)

    let private getNullableValue
        : (JsonElement -> bool) -> (JsonElement -> 'value | null) -> JsonElement -> 'value option =
        fun isValue getRawValue -> pureIf isValue >> Option.bind (getRawValue >> NullOpt.toOption)

    let private getValue: (JsonElement -> bool) -> (JsonElement -> 'value) -> JsonElement -> 'value option =
        fun isValue getRawValue -> pureIf isValue >> Option.map getRawValue


    let getBool: JsonElement -> bool option = getValue isBool _.GetBoolean()


    let getSByte: JsonElement -> sbyte option = getRefValue isNumber _.TryGetSByte()
    let getInt16: JsonElement -> int16 option = getRefValue isNumber _.TryGetInt16()
    let getInt: JsonElement -> int option = getRefValue isNumber _.TryGetInt32()
    let getInt64: JsonElement -> int64 option = getRefValue isNumber _.TryGetInt64()
    let getByte: JsonElement -> byte option = getRefValue isNumber _.TryGetByte()
    let getUInt16: JsonElement -> uint16 option = getRefValue isNumber _.TryGetUInt16()
    let getUInt: JsonElement -> uint option = getRefValue isNumber _.TryGetUInt32()
    let getUInt64: JsonElement -> uint64 option = getRefValue isNumber _.TryGetUInt64()

    let getFloat32: JsonElement -> float32 option =
        getRefValue isNumber _.TryGetSingle()

    let getFloat: JsonElement -> float option = getRefValue isNumber _.TryGetDouble()

    let getDecimal: JsonElement -> decimal option =
        getRefValue isNumber _.TryGetDecimal()


    let getBase64: JsonElement -> byte array option =
        getRefNullableValue isString _.TryGetBytesFromBase64()

    /// `DateTime.Nanosecond` cannot be greater than 900,
    /// in which case the `DateTime.Microsecond` will carry up.
    let getDateTime: JsonElement -> System.DateTime option =
        getRefValue isString _.TryGetDateTime()

    /// `DateTime.Nanosecond` cannot be greater than 900,
    /// in which case the `DateTime.Microsecond` will carry up.
    let getDateTimeOffset: JsonElement -> System.DateTimeOffset option =
        getRefValue isString _.TryGetDateTimeOffset()

    let getString: JsonElement -> string option =
        getNullableValue isString _.GetString()

    let getGuid: JsonElement -> System.Guid option = getRefValue isString _.TryGetGuid()


    let getRawText: JsonElement -> string = _.GetRawText()

    let getInt128: JsonElement -> System.Int128 option =
        getRefValue isNumber (getRawText >> System.Int128.TryParse)

    let getUInt128: JsonElement -> System.UInt128 option =
        getRefValue isNumber (getRawText >> System.UInt128.TryParse)


    let getArrayEnumerator: JsonElement -> JsonElement.ArrayEnumerator option =
        getValue isArray _.EnumerateArray()

    let getProperty: string -> JsonElement -> JsonElement option =
        fun field -> getRefValue isObject _.TryGetProperty(field)
