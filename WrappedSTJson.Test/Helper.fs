namespace WrappedSTJson.Test

open Expecto

module Helper =
    let inline testCaseId (name: string) (test: string -> unit) : Test = testCase name (fun () -> test name)

    // This code is partially licensed under the Apache License 2.0
    // The functions with URLs use that license.
    // Original copyright notice:
    // Copyright (c) 2016, github.com/haf/expecto contributors
    // License details: http://www.apache.org/licenses/LICENSE-2.0

    // https://github.com/haf/expecto/issues/413#issue-836142831
    // https://github.com/haf/expecto/blob/10.2.1/Expecto/Expect.fs#L277-L290
    // https://github.com/haf/expecto/blob/10.2.1/README.md?plain=1#L933-L939
    let inline private floatLikeClose
        (isInfinity: ^a -> bool)
        (accuracyAbsolute: ^a)
        (accuracyRelative: ^a)
        (actual: ^a)
        (expected: ^a)
        (message: string)
        : unit =
        if isInfinity actual then
            failtestf "%s. Expected actual to not be infinity, but it was." message
        elif isInfinity expected then
            failtestf "%s. Expected expected to not be infinity, but it was." message
        elif
            abs (actual - expected)
            <= accuracyAbsolute + accuracyRelative * max (abs actual) (abs expected)
            |> not
        then
            failtestf
                "%s. Expected difference to be less than %.20g for accuracy (absolute=%.20g relative=%.20g), but was %.20g. actual=%.20g expected=%.20g"
                message
                (accuracyAbsolute + accuracyRelative * max (abs actual) (abs expected))
                accuracyAbsolute
                accuracyRelative
                (actual - expected)
                actual
                expected

    let private accuracyDecimalAbsolute: decimal = decimal Accuracy.veryHigh.absolute
    let private accuracyDecimalRelative: decimal = decimal Accuracy.veryHigh.relative
    let private accuracyFloatAbsolute: float = Accuracy.veryHigh.absolute
    let private accuracyFloatRelative: float = Accuracy.veryHigh.relative
    let private accuracyFloat32Absolute: float32 = float32 Accuracy.low.absolute
    let private accuracyFloat32Relative: float32 = float32 Accuracy.low.relative

    let floatEq: float -> float -> string -> unit =
        fun actual expect message ->
            floatLikeClose System.Double.IsInfinity accuracyFloatAbsolute accuracyFloatRelative actual expect message

    let float32Eq: float32 -> float32 -> string -> unit =
        fun actual expect message ->
            floatLikeClose
                System.Single.IsInfinity
                accuracyFloat32Absolute
                accuracyFloat32Relative
                actual
                expect
                message

    let decimalEq: decimal -> decimal -> string -> unit =
        fun actual expect message ->
            floatLikeClose (fun _ -> false) accuracyDecimalAbsolute accuracyDecimalRelative actual expect message
