﻿module IInputOutPut

type IInputOut = 
    abstract member print : list<string> -> unit
    abstract member getUserInput : unit -> string

