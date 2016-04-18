﻿module ScreenEdit

let writeToScreen(ticTacToeBox : array<string>) (message : string) =
    System.Console.Clear()
    printfn "%s" message
    printfn "------------------------"
    printfn "|  %s   |   %s   |   %s   |" (string ticTacToeBox.[0]) (string ticTacToeBox.[1]) (string ticTacToeBox.[2])
    printfn "------------------------"
    printfn "|  %s   |   %s   |   %s   |" (string ticTacToeBox.[3]) (string ticTacToeBox.[4]) (string ticTacToeBox.[5])
    printfn "------------------------"
    printfn "|  %s   |   %s   |   %s   |" (string ticTacToeBox.[6]) (string ticTacToeBox.[7]) (string ticTacToeBox.[8])
    printfn "------------------------"