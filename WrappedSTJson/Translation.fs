namespace WrappedSTJson

module Translation =
    let getBytes: string -> byte array =
        fun str ->
            let outputArray =
                str.Length |> System.Text.UTF8Encoding.UTF8.GetMaxByteCount |> Array.zeroCreate

            let outputSpan = System.Span<byte> outputArray

            match System.Text.UTF8Encoding.UTF8.TryGetBytes(str, outputSpan) with
            | true, bytesWritten ->
                if outputArray.Length = bytesWritten then
                    outputArray
                else
                    // range is inclusive
                    outputArray[.. bytesWritten - 1]
            | false, _ -> failwith "Unreachable!: The destination was too small to contain all the encoded bytes."

    let private getMaxLengthArray
        (sourceReadOnlySpan: System.ReadOnlySpan<byte>)
        (getMaxLength: int -> int)
        : byte array =
        sourceReadOnlySpan.Length |> getMaxLength |> Array.zeroCreate<byte>

    // There may be excessive commonality
    let private getResult
        : System.ReadOnlySpan<byte>
              -> System.Span<byte>
              -> System.Buffers.OperationStatus * int * int
              -> Result<string, string> =
        fun inputReadOnlySpan outputBytesSpan res ->
            match res with
            | operationStatus, _, _ when operationStatus <> System.Buffers.OperationStatus.Done ->
                sprintf
                    "expect operationStatus = System.Buffers.OperationStatus.Done, but operationStatus is %s."
                    (string operationStatus)
                |> Error
            | _, consumed, _ when inputReadOnlySpan.Length <> consumed ->
                sprintf
                    "expect inputReadOnlySpan.Length = consumed, but inputReadOnlySpan.Length is %d and consumed is %d."
                    inputReadOnlySpan.Length
                    consumed
                |> Error
            | _, _, outputByteCount when outputBytesSpan.Length <> outputByteCount ->
                let outputBytesSpan = outputBytesSpan.Slice(0, outputByteCount)
                System.Text.UTF8Encoding.UTF8.GetString outputBytesSpan |> Ok
            | _, _, _ -> System.Text.UTF8Encoding.UTF8.GetString outputBytesSpan |> Ok

    let encodeToBase64: string -> Result<string, string> =
        fun sourceString ->
            let sourceArray = getBytes sourceString
            let sourceSpan = System.ReadOnlySpan<byte> sourceArray

            let encodedBytesArray =
                getMaxLengthArray sourceSpan System.Buffers.Text.Base64.GetMaxEncodedToUtf8Length

            let encodedBytesSpan = System.Span<byte> encodedBytesArray

            let tupled = System.Buffers.Text.Base64.EncodeToUtf8(sourceSpan, encodedBytesSpan)
            getResult sourceSpan encodedBytesSpan tupled

    let decodeFromBase64: string -> Result<string, string> =
        fun sourceString ->
            let sourceArray = getBytes sourceString
            let sourceSpan = System.ReadOnlySpan<byte> sourceArray

            let decodedBytesArray =
                getMaxLengthArray sourceSpan System.Buffers.Text.Base64.GetMaxDecodedFromUtf8Length

            let decodedBytesSpan = System.Span<byte> decodedBytesArray

            let tupled = System.Buffers.Text.Base64.DecodeFromUtf8(sourceSpan, decodedBytesSpan)
            getResult sourceSpan decodedBytesSpan tupled
