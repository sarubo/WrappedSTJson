namespace WrappedSTJson.Test

open Expecto
open WrappedSTJson
open WrappedSTJson.Test.Helper
open System.Text.Json

module JsonElmTest =
    let private testString = "髙橋・いろはﾏｹﾄﾞﾆｱ👩‍👩‍👦‍👦👩‍👩‍👦‍👦🇲🇰"
    let private testGuidOfD = "c073cc31-9ec0-4d62-83eb-16edd2b26f90"
    let private testDateTime = "2006-01-02T15:04:05.9999999Z"

    let private jsonExample =
        $$"""{
    "Object": {},
    "Array": [],
    "String": "",
    "Number": 0.0,
    "True": true,
    "False": false,
    "Null": null,
    "System.SByte.MaxValue": {{System.SByte.MaxValue}},
    "System.SByte.MinValue": {{System.SByte.MinValue}},
    "System.Int16.MaxValue": {{System.Int16.MaxValue}},
    "System.Int16.MinValue": {{System.Int16.MinValue}},
    "System.Int32.MaxValue": {{System.Int32.MaxValue}},
    "System.Int32.MinValue": {{System.Int32.MinValue}},
    "System.Int64.MaxValue": {{System.Int64.MaxValue}},
    "System.Int64.MinValue": {{System.Int64.MinValue}},
    "System.Byte.MaxValue": {{System.Byte.MaxValue}},
    "System.Byte.MinValue": {{System.Byte.MinValue}},
    "System.UInt16.MaxValue": {{System.UInt16.MaxValue}},
    "System.UInt16.MinValue": {{System.UInt16.MinValue}},
    "System.UInt32.MaxValue": {{System.UInt32.MaxValue}},
    "System.UInt32.MinValue": {{System.UInt32.MinValue}},
    "System.UInt64.MaxValue": {{System.UInt64.MaxValue}},
    "System.UInt64.MinValue": {{System.UInt64.MinValue}},
    "System.Single.Pi": {{System.Single.Pi}},
    "System.Double.Pi": {{System.Double.Pi}},
    "decimal System.Double.Pi * decimal System.Double.Pi": {{decimal System.Double.Pi * decimal System.Double.Pi}},
    "Base64": "{{testString |> Translation.encodeToBase64 |> Result.defaultValue "Error"}}",
    "UTC System.DateTime": "{{testDateTime}}",
    "StringWithEmoji": "{{testString}}",
    "System.Guid.ToString D": "{{testGuidOfD}}",
    "System.Int128.MaxValue": {{System.Int128.MaxValue}},
    "System.Int128.MinValue": {{System.Int128.MinValue}},
    "System.UInt128.MaxValue": {{System.UInt128.MaxValue}},
    "System.UInt128.MinValue": {{System.UInt128.MinValue}},
    "ArrayOf foo bar baz": [ "foo", "bar", "baz" ]
}
"""

    [<Tests>]
    let tests =
        testList "Check JsonElm" [
            testCaseId "JsonElm.isObject: {} is true"
            <| fun message ->
                use doc = JsonDocument.Parse "{}"
                let actual = doc.RootElement |> JsonElm.isObject
                Expect.isTrue actual message

            testCaseId "JsonElm.isArray: [] is true"
            <| fun message ->
                use doc = JsonDocument.Parse "[]"
                let actual = doc.RootElement |> JsonElm.isArray
                Expect.isTrue actual message

            testCaseId "JsonElm.getProperty: get Object and {} is Object"
            <| fun message ->
                use doc = JsonDocument.Parse jsonExample

                let actual =
                    doc.RootElement |> JsonElm.getProperty "Object" |> Option.map JsonElm.isObject

                Expect.isSome actual message
                actual |> Option.iter (fun actual -> Expect.isTrue actual message)

            testCaseId "JsonElm.getProperty: get Undefine and it is None"
            <| fun message ->
                use doc = JsonDocument.Parse jsonExample
                let actual = doc.RootElement |> JsonElm.getProperty "Undefine"
                Expect.isNone actual message

            // `isUndefined` is always false because `JsonElm.getProperty` provides `undefined` as `None`.
            testCaseId "JsonElm.isUndefined: {} is false"
            <| fun message ->
                use doc = JsonDocument.Parse "{}"
                let actual = doc.RootElement |> JsonElm.isUndefined
                Expect.isFalse actual message

            testCaseId "JsonElm.isString: \"\" is String"
            <| fun message ->
                use doc = JsonDocument.Parse jsonExample

                let actual =
                    doc.RootElement |> JsonElm.getProperty "String" |> Option.map JsonElm.isString

                Expect.isSome actual message
                actual |> Option.iter (fun actual -> Expect.isTrue actual message)

            testCaseId "JsonElm.isNumber: 0.0 is Number"
            <| fun message ->
                use doc = JsonDocument.Parse jsonExample

                let actual =
                    doc.RootElement |> JsonElm.getProperty "Number" |> Option.map JsonElm.isNumber

                Expect.isSome actual message
                actual |> Option.iter (fun actual -> Expect.isTrue actual message)

            testCaseId "JsonElm.isNull: null is null"
            <| fun message ->
                use doc = JsonDocument.Parse jsonExample

                let actual =
                    doc.RootElement |> JsonElm.getProperty "Null" |> Option.map JsonElm.isNull

                Expect.isSome actual message
                actual |> Option.iter (fun actual -> Expect.isTrue actual message)

            testCaseId "JsonElm.isBool: True is true"
            <| fun message ->
                use doc = JsonDocument.Parse jsonExample

                let actual =
                    doc.RootElement |> JsonElm.getProperty "True" |> Option.map JsonElm.isBool

                Expect.isSome actual message
                actual |> Option.iter (fun actual -> Expect.isTrue actual message)

            testCaseId "JsonElm.isBool: False is true"
            <| fun message ->
                use doc = JsonDocument.Parse jsonExample

                let actual =
                    doc.RootElement |> JsonElm.getProperty "False" |> Option.map JsonElm.isBool

                Expect.isSome actual message
                actual |> Option.iter (fun actual -> Expect.isTrue actual message)

            testCaseId "JsonElm.getBool: True is true"
            <| fun message ->
                use doc = JsonDocument.Parse jsonExample

                let actual =
                    doc.RootElement |> JsonElm.getProperty "True" |> Option.bind JsonElm.getBool

                Expect.isSome actual message
                actual |> Option.iter (fun actual -> Expect.isTrue actual message)

            testCaseId "JsonElm.getBool: False is false"
            <| fun message ->
                use doc = JsonDocument.Parse jsonExample

                let actual =
                    doc.RootElement |> JsonElm.getProperty "False" |> Option.bind JsonElm.getBool

                Expect.isSome actual message
                actual |> Option.iter (fun actual -> Expect.isFalse actual message)

            testCaseId "JsonElm.getSByte: System.SByte.MaxValue is <id>"
            <| fun message ->
                use doc = JsonDocument.Parse jsonExample

                let actual =
                    doc.RootElement
                    |> JsonElm.getProperty "System.SByte.MaxValue"
                    |> Option.bind JsonElm.getSByte

                Expect.isSome actual message

                actual
                |> Option.iter (fun actual -> Expect.equal actual System.SByte.MaxValue message)

            testCaseId "JsonElm.getSByte: System.SByte.MinValue is <id>"
            <| fun message ->
                use doc = JsonDocument.Parse jsonExample

                let actual =
                    doc.RootElement
                    |> JsonElm.getProperty "System.SByte.MinValue"
                    |> Option.bind JsonElm.getSByte

                Expect.isSome actual message

                actual
                |> Option.iter (fun actual -> Expect.equal actual System.SByte.MinValue message)

            testCaseId "JsonElm.getInt16: System.Int16.MaxValue is <id>"
            <| fun message ->
                use doc = JsonDocument.Parse jsonExample

                let actual =
                    doc.RootElement
                    |> JsonElm.getProperty "System.Int16.MaxValue"
                    |> Option.bind JsonElm.getInt16

                Expect.isSome actual message

                actual
                |> Option.iter (fun actual -> Expect.equal actual System.Int16.MaxValue message)

            testCaseId "JsonElm.getInt16: System.Int16.MinValue is <id>"
            <| fun message ->
                use doc = JsonDocument.Parse jsonExample

                let actual =
                    doc.RootElement
                    |> JsonElm.getProperty "System.Int16.MinValue"
                    |> Option.bind JsonElm.getInt16

                Expect.isSome actual message

                actual
                |> Option.iter (fun actual -> Expect.equal actual System.Int16.MinValue message)

            testCaseId "JsonElm.getInt: System.Int32.MaxValue is <id>"
            <| fun message ->
                use doc = JsonDocument.Parse jsonExample

                let actual =
                    doc.RootElement
                    |> JsonElm.getProperty "System.Int32.MaxValue"
                    |> Option.bind JsonElm.getInt

                Expect.isSome actual message

                actual
                |> Option.iter (fun actual -> Expect.equal actual System.Int32.MaxValue message)

            testCaseId "JsonElm.getInt: System.Int32.MinValue is <id>"
            <| fun message ->
                use doc = JsonDocument.Parse jsonExample

                let actual =
                    doc.RootElement
                    |> JsonElm.getProperty "System.Int32.MinValue"
                    |> Option.bind JsonElm.getInt

                Expect.isSome actual message

                actual
                |> Option.iter (fun actual -> Expect.equal actual System.Int32.MinValue message)

            testCaseId "JsonElm.getInt64: System.Int64.MaxValue is <id>"
            <| fun message ->
                use doc = JsonDocument.Parse jsonExample

                let actual =
                    doc.RootElement
                    |> JsonElm.getProperty "System.Int64.MaxValue"
                    |> Option.bind JsonElm.getInt64

                Expect.isSome actual message

                actual
                |> Option.iter (fun actual -> Expect.equal actual System.Int64.MaxValue message)

            testCaseId "JsonElm.getInt64: System.Int64.MinValue is <id>"
            <| fun message ->
                use doc = JsonDocument.Parse jsonExample

                let actual =
                    doc.RootElement
                    |> JsonElm.getProperty "System.Int64.MinValue"
                    |> Option.bind JsonElm.getInt64

                Expect.isSome actual message

                actual
                |> Option.iter (fun actual -> Expect.equal actual System.Int64.MinValue message)

            testCaseId "JsonElm.getByte: System.Byte.MaxValue is <id>"
            <| fun message ->
                use doc = JsonDocument.Parse jsonExample

                let actual =
                    doc.RootElement
                    |> JsonElm.getProperty "System.Byte.MaxValue"
                    |> Option.bind JsonElm.getByte

                Expect.isSome actual message

                actual
                |> Option.iter (fun actual -> Expect.equal actual System.Byte.MaxValue message)

            testCaseId "JsonElm.getByte: System.Byte.MinValue is <id>"
            <| fun message ->
                use doc = JsonDocument.Parse jsonExample

                let actual =
                    doc.RootElement
                    |> JsonElm.getProperty "System.Byte.MinValue"
                    |> Option.bind JsonElm.getByte

                Expect.isSome actual message

                actual
                |> Option.iter (fun actual -> Expect.equal actual System.Byte.MinValue message)

            testCaseId "JsonElm.getUInt16: System.UInt16.MaxValue is <id>"
            <| fun message ->
                use doc = JsonDocument.Parse jsonExample

                let actual =
                    doc.RootElement
                    |> JsonElm.getProperty "System.UInt16.MaxValue"
                    |> Option.bind JsonElm.getUInt16

                Expect.isSome actual message

                actual
                |> Option.iter (fun actual -> Expect.equal actual System.UInt16.MaxValue message)

            testCaseId "JsonElm.getUInt16: System.UInt16.MinValue is <id>"
            <| fun message ->
                use doc = JsonDocument.Parse jsonExample

                let actual =
                    doc.RootElement
                    |> JsonElm.getProperty "System.UInt16.MinValue"
                    |> Option.bind JsonElm.getUInt16

                Expect.isSome actual message

                actual
                |> Option.iter (fun actual -> Expect.equal actual System.UInt16.MinValue message)

            testCaseId "JsonElm.getUInt: System.UInt32.MaxValue is <id>"
            <| fun message ->
                use doc = JsonDocument.Parse jsonExample

                let actual =
                    doc.RootElement
                    |> JsonElm.getProperty "System.UInt32.MaxValue"
                    |> Option.bind JsonElm.getUInt

                Expect.isSome actual message

                actual
                |> Option.iter (fun actual -> Expect.equal actual System.UInt32.MaxValue message)

            testCaseId "JsonElm.getUInt: System.UInt32.MinValue is <id>"
            <| fun message ->
                use doc = JsonDocument.Parse jsonExample

                let actual =
                    doc.RootElement
                    |> JsonElm.getProperty "System.UInt32.MinValue"
                    |> Option.bind JsonElm.getUInt

                Expect.isSome actual message

                actual
                |> Option.iter (fun actual -> Expect.equal actual System.UInt32.MinValue message)

            testCaseId "JsonElm.getUInt64: System.UInt64.MaxValue is <id>"
            <| fun message ->
                use doc = JsonDocument.Parse jsonExample

                let actual =
                    doc.RootElement
                    |> JsonElm.getProperty "System.UInt64.MaxValue"
                    |> Option.bind JsonElm.getUInt64

                Expect.isSome actual message

                actual
                |> Option.iter (fun actual -> Expect.equal actual System.UInt64.MaxValue message)

            testCaseId "JsonElm.getUInt64: System.UInt64.MinValue is <id>"
            <| fun message ->
                use doc = JsonDocument.Parse jsonExample

                let actual =
                    doc.RootElement
                    |> JsonElm.getProperty "System.UInt64.MinValue"
                    |> Option.bind JsonElm.getUInt64

                Expect.isSome actual message

                actual
                |> Option.iter (fun actual -> Expect.equal actual System.UInt64.MinValue message)

            testCaseId "JsonElm.getFloat32: System.Single.Pi is <id>"
            <| fun message ->
                use doc = JsonDocument.Parse jsonExample

                let actual =
                    doc.RootElement
                    |> JsonElm.getProperty "System.Single.Pi"
                    |> Option.bind JsonElm.getFloat32

                Expect.isSome actual message
                actual |> Option.iter (fun actual -> float32Eq actual System.Single.Pi message)

            testCaseId "JsonElm.getFloat: System.Double.Pi is <id>"
            <| fun message ->
                use doc = JsonDocument.Parse jsonExample

                let actual =
                    doc.RootElement
                    |> JsonElm.getProperty "System.Double.Pi"
                    |> Option.bind JsonElm.getFloat

                Expect.isSome actual message
                actual |> Option.iter (fun actual -> floatEq actual System.Double.Pi message)

            testCaseId "JsonElm.getDecimal: decimal System.Double.Pi * decimal System.Double.Pi is <id>"
            <| fun message ->
                use doc = JsonDocument.Parse jsonExample

                let actual =
                    doc.RootElement
                    |> JsonElm.getProperty "decimal System.Double.Pi * decimal System.Double.Pi"
                    |> Option.bind JsonElm.getDecimal

                let expect = decimal System.Double.Pi * decimal System.Double.Pi

                Expect.isSome actual message
                actual |> Option.iter (fun actual -> decimalEq actual expect message)

            testCaseId "JsonElm.getBytes: Base64 is testString"
            <| fun message ->
                use doc = JsonDocument.Parse jsonExample

                let actual =
                    doc.RootElement |> JsonElm.getProperty "Base64" |> Option.bind JsonElm.getBase64

                let expect = testString |> Translation.getBytes

                Expect.isSome actual message
                actual |> Option.iter (fun actual -> Expect.equal actual expect message)

            testCaseId "JsonElm.getDateTime: UTC System.DateTime is UTC testDateTime"
            <| fun message ->
                use doc = JsonDocument.Parse jsonExample

                let actual =
                    doc.RootElement
                    |> JsonElm.getProperty "UTC System.DateTime"
                    |> Option.bind JsonElm.getDateTime

                let expect =
                    System.DateTime.Parse(
                        // golang format
                        testDateTime,
                        System.Globalization.CultureInfo.InvariantCulture,
                        System.Globalization.DateTimeStyles.AdjustToUniversal
                    )

                Expect.isSome actual message

                actual |> Option.iter (fun actual -> Expect.equal actual expect message)

            testCaseId "JsonElm.getDateTimeOffset: UTC System.DateTime is UTC testDateTime"
            <| fun message ->
                use doc = JsonDocument.Parse jsonExample

                let actual =
                    doc.RootElement
                    |> JsonElm.getProperty "UTC System.DateTime"
                    |> Option.bind JsonElm.getDateTimeOffset

                let expect =
                    System.DateTimeOffset.Parse(
                        // golang format
                        testDateTime,
                        System.Globalization.CultureInfo.InvariantCulture,
                        System.Globalization.DateTimeStyles.AdjustToUniversal
                    )

                Expect.isSome actual message

                actual |> Option.iter (fun actual -> Expect.equal actual expect message)

            testCaseId "JsonElm.getString: StringWithEmoji is testString"
            <| fun message ->
                use doc = JsonDocument.Parse jsonExample

                let actual =
                    doc.RootElement
                    |> JsonElm.getProperty "StringWithEmoji"
                    |> Option.bind JsonElm.getString

                let expect = testString

                Expect.isSome actual message

                actual |> Option.iter (fun actual -> Expect.equal actual expect message)

            testCaseId "JsonElm.getGuid: System.Guid.ToString D is testGuidOfD"
            <| fun message ->
                use doc = JsonDocument.Parse jsonExample

                let actual =
                    doc.RootElement
                    |> JsonElm.getProperty "System.Guid.ToString D"
                    |> Option.bind JsonElm.getGuid

                let expect = testGuidOfD |> System.Guid.Parse

                Expect.isSome actual message

                actual |> Option.iter (fun actual -> Expect.equal actual expect message)

            testCaseId "JsonElm.getRawText: StringWithEmoji is <id>"
            <| fun message ->
                use doc = JsonDocument.Parse jsonExample

                let actual =
                    doc.RootElement
                    |> JsonElm.getProperty "StringWithEmoji"
                    |> Option.map JsonElm.getRawText

                let expect = $"\"{testString}\""

                Expect.isSome actual message

                actual |> Option.iter (fun actual -> Expect.equal actual expect message)

            testCaseId "JsonElm.getInt128: System.Int128.MaxValue is <id>"
            <| fun message ->
                use doc = JsonDocument.Parse jsonExample

                let actual =
                    doc.RootElement
                    |> JsonElm.getProperty "System.Int128.MaxValue"
                    |> Option.bind JsonElm.getInt128

                Expect.isSome actual message

                actual
                |> Option.iter (fun actual -> Expect.equal actual System.Int128.MaxValue message)

            testCaseId "JsonElm.getInt128: System.Int128.MinValue is <id>"
            <| fun message ->
                use doc = JsonDocument.Parse jsonExample

                let actual =
                    doc.RootElement
                    |> JsonElm.getProperty "System.Int128.MinValue"
                    |> Option.bind JsonElm.getInt128

                Expect.isSome actual message

                actual
                |> Option.iter (fun actual -> Expect.equal actual System.Int128.MinValue message)

            testCaseId "JsonElm.getUInt128: System.UInt128.MaxValue is <id>"
            <| fun message ->
                use doc = JsonDocument.Parse jsonExample

                let actual =
                    doc.RootElement
                    |> JsonElm.getProperty "System.UInt128.MaxValue"
                    |> Option.bind JsonElm.getUInt128

                Expect.isSome actual message

                actual
                |> Option.iter (fun actual -> Expect.equal actual System.UInt128.MaxValue message)

            testCaseId "JsonElm.getUInt128: System.UInt128.MinValue is <id>"
            <| fun message ->
                use doc = JsonDocument.Parse jsonExample

                let actual =
                    doc.RootElement
                    |> JsonElm.getProperty "System.UInt128.MinValue"
                    |> Option.bind JsonElm.getUInt128

                Expect.isSome actual message

                actual
                |> Option.iter (fun actual -> Expect.equal actual System.UInt128.MinValue message)

            testCaseId "JsonElm.getArrayEnumerator: ArrayOf foo bar baz is [|foo; bar; baz|]"
            <| fun message ->
                use doc = JsonDocument.Parse jsonExample

                let actual =
                    doc.RootElement
                    |> JsonElm.getProperty "ArrayOf foo bar baz"
                    |> Option.bind JsonElm.getArrayEnumerator

                let expect = [| "foo"; "bar"; "baz" |]

                Expect.isSome actual message

                actual
                |> Option.map (NullOpt.filterMap JsonElm.getString >> Seq.toArray)
                |> Option.iter (fun actual -> Expect.equal actual expect message)
        ]
