using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

[System.Serializable]
public struct SaveData
{
    [JsonProperty("HighScore")]
    public int Record { get; set; } 

    [JsonProperty("PlayerName")]
    public string PlayerName { get; set; } 

    [JsonProperty("DifficultyLevel")]
    public string DifficultyLevel { get; set; } 

    public SaveData(int score, string name, string difficulty)
    {
        PlayerName = name;
        Record = score;
        DifficultyLevel = difficulty;
    }
}

public class Results
{
    private List<SaveData> _playersResults;
    private const string FilePath = @"E:\SnakeGame\SnakeGame.json";

    public Results()
    {
        _playersResults = new List<SaveData>();
        LoadAllRecords();
    }
    public List<SaveData> getPlayersResults() 
    {
        return _playersResults;
    }

    public void SaveRecord(string name, int score, string difficulty)
    {
        var newRecord = new SaveData(score, name, difficulty);
        bool isUpdated = false;

        for (int i = 0; i < _playersResults.Count; i++)
        {
            if (_playersResults[i].PlayerName == name &&
                _playersResults[i].DifficultyLevel == difficulty &&
                score > _playersResults[i].Record)
            {
                _playersResults[i] = newRecord; // Обновляем рекорд
                isUpdated = true;
                break;
            }
        }

        if (!isUpdated)
        {
            _playersResults.Add(newRecord); // Добавляем новый рекорд
        }

        SaveToFile();
    }

    private void SaveToFile()
    {
        string json = JsonConvert.SerializeObject(_playersResults, Formatting.Indented);

        // Используем using для автоматического освобождения ресурсов
        using (StreamWriter writer = new StreamWriter(FilePath))
        {
            writer.Write(json);
        }
    }

    public void LoadAllRecords()
    {
        if (File.Exists(FilePath))
        {
            try
            {
                var json = File.ReadAllText(FilePath);
                var data = JsonConvert.DeserializeObject<List<SaveData>>(json);

                if (data != null) // Проверка на null
                {
                    _playersResults.AddRange(data);
                }
            }
            catch (JsonException ex)
            {
                // Логируем ошибку или обрабатываем ее по вашему усмотрению
                Console.WriteLine($"Ошибка при загрузке данных: {ex.Message}");
            }
            catch (IOException ex)
            {
                // Логируем ошибку или обрабатываем ее по вашему усмотрению
                Console.WriteLine($"Ошибка ввода-вывода: {ex.Message}");
            }
        }
    }
}
