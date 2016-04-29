﻿module Game
open CleanInput
open TicTacToeBoxEdit
open AI
open CheckForWinnerOrTie
open ScreenEdit
open GameSettings
open GameStatusCodes
open userInputException
open PlayerValues
open IInputOutPut
open InputOutPut

exception GameStillInSession of string

let isInputANumber(postion : string) : bool =
    try
        let posAsInt = int postion
        true
    with
        | :? System.FormatException -> false


let isOutBounds(arrayLenth : int)(postion : int) : bool =
    if arrayLenth < postion || postion < 0 then
        true
    else
        false

let isSpotTaken(ticTacToeBox : array<string>)
               (postion : int)
               (playerGlyph : string) 
               (aIGlyph : string)
               : bool =
    if ticTacToeBox.[postion] = playerGlyph || ticTacToeBox.[postion] = aIGlyph then
        true
    else
        false
        
let isUserInputCorrect(ticTacToeBox : array<string>)
                      (postion : string)
                      (playerGlyph : string) 
                      (aIGlyph : string) : string =
    if not (isInputANumber(postion)) then
        "Value not a number"
    elif isOutBounds(ticTacToeBox.Length)(int postion - 1) then
        "Value out of bounds"
    elif isSpotTaken(ticTacToeBox)(int postion - 1)(playerGlyph)(aIGlyph) then
        "Spot Taken"
    else
        ""
let convertStringToInt(pos : string) : int =
    int pos

let userInputEditTicTacToeBox(game : gameSetting)(postion : string) : string = 
    let message = isUserInputCorrect(game.ticTacToeBox)
                    (postion)(game.playerGlyph)(game.aIGlyph)
    if( message = "" ) then
        insertUserOption(game.ticTacToeBox)
                        (convertStringToInt(postion))
                        (game.playerGlyph)
                        (game.aIGlyph)
        ""
    else
        message


let aIInputEditTicTacToeBox(game : gameSetting) : bool =
    aIMove(game)
    true

let isGameOver(game : gameSetting) : bool =
    if checkForWinnerOrTie(game.ticTacToeBox)(game.playerGlyph)(game.aIGlyph) 
            = int GenResult.NoWinner then
        false
    else
        true

let humanVsAiGame(game : gameSetting) (io : IInputOut) : int =
    writeToScreen(game.ticTacToeBox)(game.inverted)("")(io) |> ignore
    let mutable userError = false
    while not (isGameOver(game)) do
        let message = 
            userInputEditTicTacToeBox(game)(io.getUserInput())
        if message = "" && userError then
           aIInputEditTicTacToeBox(game) |> ignore
        else
            userError <- true
        writeToScreen(game.ticTacToeBox)(game.inverted)(message)(io) |> ignore
    checkForWinnerOrTie(game.ticTacToeBox)(game.playerGlyph)(game.aIGlyph) 
            

let startGame (game : gameSetting) : int =
    let io = new InputOut()
    humanVsAiGame(game)(io)

(*
let aIPlayer(ticTacToeBox : array<string>)(gameOption : gameSetting) : int =
    let mutable place = 0
    for i = 0 to ticTacToeBox.Length - 1 do
        if ticTacToeBox.[i] = gameOption.playerGlyph then
            ticTacToeBox.[i] <- gameOption.aIGlyph
        elif ticTacToeBox.[i] = gameOption.aIGlyph then
            ticTacToeBox.[i] <- gameOption.playerGlyph
    place <- computerMove(ticTacToeBox)(gameOption)
    for i = 0 to ticTacToeBox.Length - 1 do
        if ticTacToeBox.[i] = gameOption.playerGlyph then
            ticTacToeBox.[i] <- gameOption.aIGlyph
        elif ticTacToeBox.[i] = gameOption.aIGlyph then
            ticTacToeBox.[i] <- gameOption.playerGlyph
    place

let isGameOver(ticTacToeBox : array<string>)(gameOption : gameSetting) : bool =
    if checkForWinnerOrTie(ticTacToeBox)(gameOption) = int GenResult.NoWinner then
        false
    else
        true

let gameEndingMessage(ticTacToeBox : array<string>)(gameOption : gameSetting) : string =
    let mutable message = ""
    let endResult = checkForWinnerOrTie(ticTacToeBox)(gameOption)
    if endResult = getWinningAIValue(ticTacToeBox) then
        message <- "AI Won"
    elif endResult = getWinningHumanValue(ticTacToeBox) then
        message <- "Human Won"
    elif endResult = int GenResult.Tie then
        message <- "Tie" 
    else
        raise(GameStillInSession("Game is still going"))
    message

let startGame (gameOption : gameSetting) : int =
    let mutable game = gameOption
    let mutable currentPlayer = game.firstPlayer
    let mutable message = ""
    let mutable userError = false
    if not game.aIvAI then
        System.Console.Clear()
        printfn "Starting game..."
    while not (isGameOver(game.ticTacToeBox)(gameOption)) do
        try
            if not game.aIvAI then
                if game.firstPlayer = int playerVals.Human then
                    writeToScreen(game) (message)
                    printf "Input: "
                    game.ticTacToeBox <- InsertUserOption (game.ticTacToeBox) 
                                            (SanitizeHumanPickedPlace 
                                            (System.Console.ReadLine())
                                            (game.ticTacToeBox.Length))(gameOption)   
                 else
                    if not userError then
                        if not game.aIvAI then
                            writeToScreen(game) ("AI is thinking...")
                        game.ticTacToeBox <- AIMove (game.ticTacToeBox)(gameOption)   
            else
                game.ticTacToeBox <- InsertUserOption (game.ticTacToeBox)(aIPlayer(game.ticTacToeBox)(gameOption) + 1)(gameOption)
            
            if not (isGameOver(game.ticTacToeBox)(gameOption)) then
                if game.firstPlayer = int playerVals.Human && not userError then
                    if not game.aIvAI then
                        writeToScreen(game) ("AI is thinking...")
                    game.ticTacToeBox <- AIMove (game.ticTacToeBox)(gameOption)   
                else
                    writeToScreen(game) (message)
                    printf "Input: "
                    game.ticTacToeBox <- InsertUserOption (game.ticTacToeBox) 
                                            (SanitizeHumanPickedPlace 
                                            (System.Console.ReadLine())
                                            (game.ticTacToeBox.Length))(gameOption)   
            if isGameOver(game.ticTacToeBox)(gameOption) then
                if not game.aIvAI then
                    writeToScreen(game) (gameEndingMessage(game.ticTacToeBox)(gameOption))
            message <- ""
            userError <- false
        with
            | :? NonIntError -> message <- "Invaild Number"; userError <- true
            | :? OutOfBoundsOverFlow ->  message <- "Value to large"; userError <- true
            | :? OutOfBoundsUnderFlow -> message <- "Value to small"; userError <- true
            | :? SpotAlreayTaken -> message <- "Spot Taken"; userError <- true
    
    checkForWinnerOrTie(game.ticTacToeBox)(gameOption)
    *)