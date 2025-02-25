namespace WrappedSTJson

module NullOpt =
    let toOption<'a when 'a: not null and 'a: not struct> : 'a | null -> 'a option =
        function
        | Null -> None
        | NonNull v -> Some v

    let isNotNull<'a when 'a: not null and 'a: not struct> : 'a | null -> bool =
        fun x -> x |> isNull |> not

    let toOptions<'a when 'a: not null and 'a: not struct> : seq<'a | null> -> seq<'a option> =
        fun xs -> xs |> Seq.map toOption

    let toNonNulls<'a when 'a: not null and 'a: not struct> : seq<'a | null> -> seq<'a> =
        fun xs -> xs |> Seq.filter isNotNull |> Seq.map nonNull

    let toFiltered<'a when 'a: not null> : seq<'a option> -> seq<'a> =
        Seq.filter Option.isSome >> Seq.map Option.get

    let filterMap<'a, 'b when 'b: not null> : ('a -> 'b option) -> seq<'a> -> seq<'b> =
        fun mapper -> Seq.map mapper >> toFiltered
