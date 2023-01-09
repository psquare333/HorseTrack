// See https://aka.ms/new-console-template for more information
using HorseTrack;

Console.WriteLine("Hello, World!");
List<HorseDetails> horses = new()
{
    new HorseDetails
    {
        Id = 1,
        Name = "That Darn Gray Cat",
        Odds = 5,
        IsWinner = true
    },
    new HorseDetails
    {
        Id = 2,
        Name = "Fort Utopia",
        Odds = 10
    },
    new HorseDetails
    {
        Id = 3,
        Name = "Count Sheep",
        Odds = 9
    },
    new HorseDetails
    {
        Id = 4,
        Name = "Ms Traitour",
        Odds = 4
    },
    new HorseDetails
    {
        Id = 5,
        Name = "Real Princess",
        Odds = 3
    },
    new HorseDetails
    {
        Id = 6,
        Name = "Pa Kettle",
        Odds = 5
    },
    new HorseDetails
    {
        Id = 7,
        Name = "Gin Stinger",
        Odds = 6
    }
};

Dictionary<int, int> inventory = new()
{
    { 1, 10 },
    { 5, 10 },
    { 10, 10 },
    { 20, 10 },
    { 100, 10 }
};

PrintInventory(inventory);

PrintHorses(horses);

TakeInput();

static void PrintInventory(Dictionary<int, int> inventory)
{
    Console.WriteLine("Inventory");
    foreach (var item in inventory)
    {
        Console.WriteLine($"${item.Key},{item.Value}");
    }
}

static void PrintHorses(List<HorseDetails> horses)
{
    Console.WriteLine("Horses");
    foreach (var horse in horses)
    {
        Console.WriteLine($"{horse.Id},{horse.Name},{horse.Odds},{(horse.IsWinner ? "won" : "lost")} ");
    }
}

void TakeInput()
{
    var input = Console.ReadLine();
    ProcessInput(input);
}

void ProcessInput(string input)
{
    if (string.IsNullOrEmpty(input))
    {
        TakeInput();
        return;
    }
    if(input.ToLower() =="q")
        Environment.Exit(0);
    if (input.ToLower() == "r")
    {
        RestockInventory();
        PrintInventory(inventory);

        PrintHorses(horses);
        TakeInput();
        return;
    }

    var inputArray = input.Split(' ');
    var commandType = "";
    var commandValue = "";
    if (inputArray.Length != 2
        || !(inputArray[0].ToLower() == "w" || 
            (Int32.TryParse(inputArray[0], out var val) && horses.Any(h => h.Id == val))))
    {
        Console.Error.WriteLine($"Invalid Command: {input}");
    }
    else
    {
        commandType = inputArray[0];
        commandValue = inputArray[1];
        if (commandType.ToLower() == "w")
        {
            ChangeWinner(commandValue);
        }
        else if (horses.Any(h => h.Id == Int32.Parse(commandType)))
        {
            var horse = horses.Where(h => h.Id == Int32.Parse(commandType)).First();
            if (horse.IsWinner)
            {
                SettlePayout(commandValue, horse);
            }
            else
            {
                Console.WriteLine($"No PayOut: {horse.Name}");
            }
        }
    }
    PrintInventory(inventory);

    PrintHorses(horses);
    TakeInput();
}

void RestockInventory()
{
    foreach (var item in inventory.Keys) {
        inventory[item]=10;
    }
}

void ChangeWinner(string commandValue)
{
    if (horses.Any(h => h.Id == Int32.Parse(commandValue)))
    {
        foreach (var horse in horses)
        {
            if (horse.Id == Int32.Parse(commandValue))
            {
                horse.IsWinner = true;
            }
            else
            {
                horse.IsWinner = false;
            }
        }
    }
}

void SettlePayout(string commandValue, HorseDetails horse)
{
    if(!int.TryParse(commandValue, out int value))
    {
        Console.WriteLine($"Invalid Bet: {commandValue}");
        return;
    }
    var payout = horse.Odds * int.Parse(commandValue);
    Dictionary<int, int> payoutSplit = new();
    foreach (var item in inventory.OrderByDescending(i => i.Key))
    {
        var payableNotes = payout / item.Key;
        payout = payout % item.Key;
        if (payableNotes <= item.Value)
        {
            payoutSplit.Add(item.Key, payableNotes);
        }
        else
        {
            payoutSplit.Add(item.Key, item.Value);
            payout +=item.Key*( payableNotes - item.Value);
        }

    }
    if (payout != 0)
    {
        Console.WriteLine($"Insufficient Funds: {horse.Odds * Int32.Parse(commandValue)}");
    }
    else
    {
        Console.WriteLine($"Payout: {horse.Name},{horse.Odds * Int32.Parse(commandValue)}");
        Console.WriteLine("Dispensing");
        foreach (var item in payoutSplit.OrderBy(i => i.Key))
        {
            Console.WriteLine($"${item.Key},{item.Value}");
            inventory[item.Key] -= item.Value;
        }
    }
}