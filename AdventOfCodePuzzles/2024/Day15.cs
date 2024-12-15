namespace AdventOfCodePuzzles._2024;

internal sealed class Day15 : BenchmarkableBase
{
    private readonly record struct Position(int X, int Y);
    
    private enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }
    
    private char[][] _map;
    
    private readonly Queue<Direction> _queue = new();
    
    private Position _startPosition;
    
    protected override void InternalOnLoad()
    {
        var map = new List<char[]>();
        var y = 0;
        for (;y < Input.Lines.Length; y++)
        {
            var line = Input.Lines[y];

            if (string.IsNullOrEmpty(line))
            {
                break;
            }

            var items = new char[line.Length];
            for (var x = 0; x < line.Length; x++)
            {
                if (line[x] == '@')
                {
                    _startPosition = new Position(x, y);
                }

                items[x] = line[x];
            }
            
            map.Add(items);
        }
        
        _map = map.ToArray();
        y++;

        foreach (var line in Input.Lines[y..])
        {
            foreach (var chr in line)
            {
                _queue.Enqueue(chr switch
                {
                    '^' => Direction.Up,
                    'v' => Direction.Down,
                    '<' => Direction.Left,
                    '>' => Direction.Right,
                });
            }
        }
    }

    protected override object InternalPart1()
    {
        var position = _startPosition;
        while (_queue.TryDequeue(out var direction))
        {
            
            var potentialNextPosition = direction switch
            {
                Direction.Up => position with { Y = position.Y - 1 },
                Direction.Down => position with { Y = position.Y + 1 },
                Direction.Left => position with { X = position.X - 1 },
                Direction.Right => position with { X = position.X + 1 },
            };

            var charAtNextPos = _map[potentialNextPosition.Y][potentialNextPosition.X];
            if (charAtNextPos is '.')
            {
                (_map[position.Y][position.X], _map[potentialNextPosition.Y][potentialNextPosition.X]) = (_map[potentialNextPosition.Y][potentialNextPosition.X], _map[position.Y][position.X]);
                position = potentialNextPosition;
                continue;
            }

            if (charAtNextPos is '#')
            {
                continue;
            }

            // Next is a box, unwind box move
            if (MoveBoxes(potentialNextPosition, direction))
            {
                (_map[position.Y][position.X], _map[potentialNextPosition.Y][potentialNextPosition.X]) = (_map[potentialNextPosition.Y][potentialNextPosition.X], _map[position.Y][position.X]);
                position = potentialNextPosition;
            }
        }

        var result = 0;
        for (var y = 0; y < _map.Length; y++)
        {
            for (var x = 0; x < _map[y].Length; x++)
            {
                if (_map[y][x] == 'O')
                {
                    result += 100 * y + x;
                }
            }
        }
        
        return result;
    }

    protected override object InternalPart2()
    {
        _startPosition = _startPosition with {X = _startPosition.X * 2};
        TransformMapForPartTwo();
        
        var position = _startPosition;
        while (_queue.TryDequeue(out var direction))
        {
            var potentialNextPosition = direction switch
            {
                Direction.Up => position with { Y = position.Y - 1 },
                Direction.Down => position with { Y = position.Y + 1 },
                Direction.Left => position with { X = position.X - 1 },
                Direction.Right => position with { X = position.X + 1 },
            };

            var charAtNextPos = _map[potentialNextPosition.Y][potentialNextPosition.X];
            if (charAtNextPos is '.')
            {
                (_map[position.Y][position.X], _map[potentialNextPosition.Y][potentialNextPosition.X]) = (_map[potentialNextPosition.Y][potentialNextPosition.X], _map[position.Y][position.X]);
                position = potentialNextPosition;
                continue;
            }

            if (charAtNextPos is '#')
            {
                continue;
            }

            // Next is a box, unwind box move
            if (MoveBoxesPartTwo(potentialNextPosition, direction))
            {
                (_map[position.Y][position.X], _map[potentialNextPosition.Y][potentialNextPosition.X]) = (_map[potentialNextPosition.Y][potentialNextPosition.X], _map[position.Y][position.X]);
                position = potentialNextPosition;
            }
        }
        
        var result = 0;
        for (var y = 0; y < _map.Length; y++)
        {
            for (var x = 0; x < _map[y].Length; x++)
            {
                if (_map[y][x] == '[')
                {
                    result += 100 * y + x;
                }
            }
        }
        
        return result;
    }

    private void TransformMapForPartTwo()
    {
        var newMap = new List<List<char>>();

        foreach (var line in _map)
        {
            var newList = new List<char>();

            foreach (var character in line)
            {
                if (character is '.' or '#')
                {
                    newList.Add(character);
                    newList.Add(character);
                }
                else if (character is '@')
                {
                    newList.Add('@');
                    newList.Add('.');
                }
                else if (character is 'O')
                {
                    newList.Add('[');
                    newList.Add(']');
                }
            }
            
            newMap.Add(newList);
        }
        

        _map = newMap.Select(x => x.ToArray()).ToArray();
    }

    private bool MoveBoxes(Position boxPosition, Direction boxDirection)
    {
        var nextBoxPosition = boxDirection switch
        {
            Direction.Up => boxPosition with { Y = boxPosition.Y - 1 },
            Direction.Down => boxPosition with { Y = boxPosition.Y + 1 },
            Direction.Left => boxPosition with { X = boxPosition.X - 1 },
            Direction.Right => boxPosition with { X = boxPosition.X + 1 },
        };

        var nextPotentialPositionCharacter = _map[nextBoxPosition.Y][nextBoxPosition.X];
        if (nextPotentialPositionCharacter == '#')
        {
            // No more moves possible
            return false;
        }

        if (nextPotentialPositionCharacter is 'O')
        {
            if (!MoveBoxes(nextBoxPosition, boxDirection))
            {
                return false;
            }
        }
        
        // Move box for real, since next space is '.'
        (_map[boxPosition.Y][boxPosition.X], _map[nextBoxPosition.Y][nextBoxPosition.X]) = (_map[nextBoxPosition.Y][nextBoxPosition.X], _map[boxPosition.Y][boxPosition.X]);
        
        return true;
    }

    private bool MoveBoxesPartTwo(Position boxPosition, Direction boxDirection)
    {
        if (boxDirection is Direction.Left or Direction.Right)
        {
            return MoveHorizontal(boxPosition, boxDirection);
        }
        
        var mapCopy = _map.Select(x => x.ToArray()).ToArray();

        if (!MoveVertical(boxPosition, boxDirection, mapCopy))
        {
            return false;
        }

        _map = mapCopy;
        return true;
    }

    private bool MoveHorizontal(Position boxPosition, Direction boxDirection)
    {
        var isLeft = boxDirection is Direction.Left;
        
        var nextPotentialPosition = boxPosition with { X = boxPosition.X + (isLeft ? -2 :  2) };
        var nextChar = _map[nextPotentialPosition.Y][nextPotentialPosition.X];

        if (nextChar is '#')
        {
            return false;
        }

        if (nextChar == (isLeft ? ']' : '['))
        {
            if (!MoveHorizontal(nextPotentialPosition, boxDirection))
            {
                return false;
            }
        }
        
        if (_map[nextPotentialPosition.Y][nextPotentialPosition.X] is '.')
        {
            _map[boxPosition.Y][boxPosition.X] = '.';

            if (isLeft)
            {
                _map[boxPosition.Y][boxPosition.X - 1] = ']';
                _map[boxPosition.Y][boxPosition.X - 2] = '[';
            }
            else
            {
                _map[boxPosition.Y][boxPosition.X + 1] = '[';
                _map[boxPosition.Y][boxPosition.X + 2] = ']';
            }
        }

        return true;
    }

    private bool MoveVertical(Position boxPosition, Direction boxDirection, char[][] mapCopy)
    {
        var isUp = boxDirection is Direction.Up;
        var newY = isUp ? boxPosition.Y - 1 : boxPosition.Y + 1;
        var line = mapCopy[newY];

        var slice = line[(boxPosition.X - 1)..(boxPosition.X + 2)];
        
        var currentChar = mapCopy[boxPosition.Y][boxPosition.X];

        bool result = true;

        if (currentChar == ']')
        {
            result = slice switch
            {
                [_, '#', _] => false,
                ['#', _, _] => false,
                ['.', '.', _] => true,
                [']', '[', _] => MoveVertical(boxPosition with {X = boxPosition.X - 1, Y = boxPosition.Y + (isUp ? -1 : 1)}, boxDirection,mapCopy ) && MoveVertical(boxPosition with {X = boxPosition.X + 1, Y = boxPosition.Y + (isUp ? -1 : 1)}, boxDirection, mapCopy),
                [_, '[', _] =>  MoveVertical(boxPosition with {X = boxPosition.X + 1, Y = boxPosition.Y + (isUp ? -1 : 1)}, boxDirection, mapCopy),
                [_, ']', _] =>  MoveVertical(boxPosition with {Y = boxPosition.Y + (isUp ? -1 : 1)}, boxDirection, mapCopy),
                [']', _, _] =>  MoveVertical(boxPosition with {X = boxPosition.X - 1, Y = boxPosition.Y + (isUp ? -1 : 1)}, boxDirection, mapCopy)
            };
        }
        else if (currentChar == '[')
        {
            result = slice switch
            {
                [_, '#', _] => false,
                [_, _, '#'] => false,
                [_, '.', '.'] => true,
                [_, ']', '['] =>  MoveVertical(boxPosition with {Y = boxPosition.Y + (isUp ? -1 : 1)}, boxDirection, mapCopy) && MoveVertical(boxPosition with {X = boxPosition.X + 1, Y = boxPosition.Y + (isUp ? -1 : 1)}, boxDirection, mapCopy),
                [_, ']', _] =>  MoveVertical(boxPosition with {Y = boxPosition.Y + (isUp ? -1 : 1)}, boxDirection, mapCopy),
                [_, '[', _] =>  MoveVertical(boxPosition with {Y = boxPosition.Y + (isUp ? -1 : 1)}, boxDirection, mapCopy),
                [_, _, '['] =>  MoveVertical(boxPosition with {X = boxPosition.X + 2, Y = boxPosition.Y + (isUp ? -1 : 1)}, boxDirection, mapCopy)
            };
        }

        if (!result)
        {
            return false;
        }

        var factors = (List<int>) [-1, 0, +1];
        if (currentChar == ']' && mapCopy[boxPosition.Y][boxPosition.X + 2] == ']')
        {
            factors.Add(2);
        }
        
        if (currentChar == '[' && mapCopy[boxPosition.Y][boxPosition.X - 1] == ']')
        {
            factors.Insert(0, -2);
        }

        factors = currentChar == '[' ? [0, 1] : [-1, 0];

        foreach (var xFactor in factors)
        {
            var x = boxPosition.X + xFactor;
            
            (mapCopy[boxPosition.Y][x], mapCopy[newY][x]) = (mapCopy[newY][x], mapCopy[boxPosition.Y][x]);
        }

        return true;
    }
    
    private void Print()
    {
        foreach (var line in _map)
        {
            Console.WriteLine(line);
        }
    }
}