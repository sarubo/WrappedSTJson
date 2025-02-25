namespace WrappedSTJson.Test

open Expecto
open WrappedSTJson
open WrappedSTJson.Test.Helper

module TranslationTest =
    let private testString = "髙橋・いろはﾏｹﾄﾞﾆｱ👩‍👩‍👦‍👦👩‍👩‍👦‍👦🇲🇰"

    [<Tests>]
    let tests =
        testList "Check Translation" [
            testCaseId "Translation.getBytes: same UTF8Encoding.UTF8.GetBytes"
            <| fun message ->
                let sourceString = testString
                let expect = System.Text.UTF8Encoding.UTF8.GetBytes sourceString
                let actual = Translation.getBytes sourceString
                Expect.equal actual expect message

            testCaseId "Translation.getBytes: null string is fail"
            <| fun message ->
                let sourceString = ""
                let expect = System.Text.UTF8Encoding.UTF8.GetBytes sourceString
                let actual = Translation.getBytes sourceString
                Expect.equal actual expect message

            testCaseId "Translation.encodeToBase64: utf8string to Base64 is OK"
            <| fun message ->
                let sourceString = testString

                let actual = Translation.encodeToBase64 sourceString

                Expect.isOk actual message

                let expect =
                    sourceString
                    |> System.Text.UTF8Encoding.UTF8.GetBytes
                    |> System.Convert.ToBase64String

                actual |> Result.iter (fun base64 -> Expect.equal base64 expect message)

            testCaseId "Translation.decodeFromBase64: utf8string from Base64 is OK"
            <| fun message ->
                let expect = testString
                let sourceArray = System.Text.UTF8Encoding.UTF8.GetBytes expect
                let sourceString = System.Convert.ToBase64String sourceArray
                let actual = Translation.decodeFromBase64 sourceString

                Expect.isOk actual message

                actual |> Result.iter (fun actual -> Expect.equal actual expect message)

            testCaseId "Translation.encodeToBase64 and Translation.decodeFromBase64: <id>"
            <| fun message ->
                let expect = testString

                let actual =
                    expect |> Translation.encodeToBase64 |> Result.bind Translation.decodeFromBase64

                Expect.isOk actual message

                actual |> Result.iter (fun actual -> Expect.equal actual expect message)
        ]
