# АНАЛИЗ ДАННЫХ И ИСКУССТВЕННЫЙ ИНТЕЛЛЕКТ [in GameDev]
Отчет по лабораторной работе #3 выполнил(а):
- Лепинских Максим Игоревич
- РИ220943

| Задание | Выполнение | Баллы |
| ------ | ------ | ------ |
| Задание 1 | * | 60 |
| Задание 2 | * | 20 |
| Задание 3 | * | 20 |

знак "*" - задание выполнено; знак "#" - задание не выполнено;

Работу проверили:
- к.т.н., доцент Денисов Д.В.
- к.э.н., доцент Панов М.А.
- ст. преп., Фадеев В.О.

[![N|Solid](https://cldup.com/dTxpPi9lDf.thumb.png)](https://nodesource.com/products/nsolid)

[![Build Status](https://travis-ci.org/joemccann/dillinger.svg?branch=master)](https://travis-ci.org/joemccann/dillinger)

Структура отчета

- Данные о работе: название работы, фио, группа, выполненные задания.
- Цель работы.
- Задание 1.
- Реализация выполнения задания. Визуализация результатов выполнения.
- Задание 2.
- Реализация выполнения задания. Визуализация результатов выполнения.
- Задание 3.
- Реализация выполнения задания. Визуализация результатов выполнения.
- Выводы.
- ✨Magic ✨

## Цель работы
Разработать оптимальный баланс для десяти уровней игры Dragon Picker

## Задание 1
### Предложите вариант изменения найденных переменных для 10-ти уровней в игре. Визуализируйте изменение уровня сложности в таблице. 

#### Найденные переменные:
- Скорость движения дракона (Speed);
- Время между сбрасыванием яиц (Time Between Egg Drops);
- Расстояние, проходимое драконом (Left Right Distance).
#### Визуализация в таблице https://docs.google.com/spreadsheets/d/1Zl0liKO-8ItIr42ZbGk0uwqAZKKmZbynLrQ6nrbrmyc/edit#gid=716382095 и здесь:
![image](https://github.com/MAXBAF1/DA-in-GameDev-lab3/assets/63009846/f209a316-714b-40bc-8704-a3dfbe421551)

## Задание 2
### Создайте 10 сцен на Unity с изменяющимся уровнем сложности.

#### Инициализатор данных
```c#
public class DataInitializer : MonoBehaviour
{
    private GSParser _parser;
    
    private void Awake()
    {
        _parser = GetComponent<GSParser>();
        StartCoroutine(_parser.ParseGS());
    }
}
```

#### Измененный код файла EnemyDragon.cs теперь включает в себя логику, которая автоматически адаптирует значения переменных в соответствии с текущим уровнем.
```c#
public class EnemyDragon : MonoBehaviour
{
    [SerializeField] private GSParser parser;
    [SerializeField] private int level;

    public GameObject dragonEggPrefab;
    public float speed;
    public float leftRightDistance;
    public float timeBetweenEggDrops;
    public float chanceDirection;

    private void Start()
    {
        Invoke(nameof(Initialize), 1f);
        Invoke(nameof(DropEgg), 2f);
    }

    private void Update()
    {
        var pos = transform.position;
        pos.x += speed * Time.deltaTime;
        transform.position = pos;

        if (pos.x < -leftRightDistance) speed = Mathf.Abs(speed);
        else if (pos.x > leftRightDistance) speed = -Mathf.Abs(speed);
    }

    private void DropEgg()
    {
        var myVector = new Vector3(0.0f, 5.0f, 0.0f);
        var egg = Instantiate(dragonEggPrefab);
        egg.transform.position = transform.position + myVector;
        Invoke(nameof(DropEgg), timeBetweenEggDrops);
    }

    private void FixedUpdate() 
    {
        if (Random.value < chanceDirection) speed *= -1;
    }

    private void Initialize()
    {
        speed = parser.Data[level][0];
		    timeBetweenEggDrops = parser.Data[level][1];
        leftRightDistance = parser.Data[level][2];
        chanceDirection = 0.01f;
    }
}
```

#### Парсер гугл таблицы:
```c#
public class GSParser : MonoBehaviour
{
    private const string Uri =
        "https://sheets.googleapis.com/v4/spreadsheets/1Zl0liKO-8ItIr42ZbGk0uwqAZKKmZbynLrQ6nrbrmyc/values/Лист2?key=AIzaSyDz9MHdX-tqBz3QH2XlvOIwXNjs3QDGKrs";

    public List<float[]> Data { get; } = new();

    public IEnumerator ParseGS()
    {
        var currResp = UnityWebRequest.Get(Uri);

        yield return currResp.SendWebRequest();
        var rawResp = currResp.downloadHandler.text;
        var rawJson = JSON.Parse(rawResp);

        foreach (var itemRawJson in rawJson["values"])
        {
            var parseJson = JSON.Parse(itemRawJson.ToString());
            var selectRow = parseJson[0].AsStringList.Skip(1);
            var count = 0;
            var arr = new float[3];

            foreach (var row in selectRow)
            {
                if (float.TryParse(row, NumberStyles.Any, CultureInfo.GetCultureInfo("ru-RU"), out var number))
                {
                    arr[count] = number;
                    count++;
                }

                if (count == 3) Data.Add(arr);
            }
        }
    }
}
```

#### 10 Сцен:
![image](https://github.com/MAXBAF1/DA-in-GameDev-lab3/assets/63009846/fd958ad9-4f01-48a5-80fc-14d30944708b)

#### 1 Сцена:
![image](https://github.com/MAXBAF1/DA-in-GameDev-lab3/assets/63009846/b11b18dd-b32a-4447-800e-c2ab0ba92599)
#### 2 Сцена:
![image](https://github.com/MAXBAF1/DA-in-GameDev-lab3/assets/63009846/d93347e8-2602-4a71-9ad5-3f99439989cd)
#### 3 Сцена:
![image](https://github.com/MAXBAF1/DA-in-GameDev-lab3/assets/63009846/d6cc1cb6-066d-4bf7-b837-b108805c9612)
#### 4 Сцена:
![image](https://github.com/MAXBAF1/DA-in-GameDev-lab3/assets/63009846/be671348-b3ba-40b9-b7c8-ecd83f45daf8)
#### 5 Сцена:
![image](https://github.com/MAXBAF1/DA-in-GameDev-lab3/assets/63009846/592fb943-8172-4ba4-b484-e84cae01fa16)
#### 6 Сцена:
![image](https://github.com/MAXBAF1/DA-in-GameDev-lab3/assets/63009846/4dee6f77-32b2-4916-870d-d9a0e1faccc1)
#### 7 Сцена:
![image](https://github.com/MAXBAF1/DA-in-GameDev-lab3/assets/63009846/35c85fbb-0752-405e-b086-86c1e012344e)
#### 8 Сцена:
![image](https://github.com/MAXBAF1/DA-in-GameDev-lab3/assets/63009846/9592570f-9257-47a9-9df6-8d452cc7aed2)
#### 9 Сцена:
![image](https://github.com/MAXBAF1/DA-in-GameDev-lab3/assets/63009846/efdfd6bb-9f44-44ef-ba91-2e3b553811a0)
#### 10 Сцена:
![image](https://github.com/MAXBAF1/DA-in-GameDev-lab3/assets/63009846/460e3a1f-27b9-4c8c-ac1b-8856766dbf19)

## Задание 3
### Решение в 80+ баллов должно заполнять google-таблицу данными из Python. В Python данные также должны быть визуализированы.
#### Код
```py
import gspread
import random

gc = gspread.service_account(filename='ad-gamedev-f8188b326391.json')
sh = gc.open("AD GameDev")

worksheet = sh.worksheet("Лист2")

def create_values(column, min_limit, max_limit, delta):
    for i in range(3, 12):
        worksheet.update(column + str(i), round(random.uniform(min_limit, max_limit), 2))
        min_limit, max_limit = min_limit + delta, max_limit + delta

create_values('B', 4, 5, 1.75)
create_values('C', 1.8, 2,-0.15)
create_values('D', 10, 11, 0.9)
```
#### Визуализация в таблице https://docs.google.com/spreadsheets/d/1Zl0liKO-8ItIr42ZbGk0uwqAZKKmZbynLrQ6nrbrmyc/edit#gid=716382095 и здесь:
![image](https://github.com/MAXBAF1/DA-in-GameDev-lab3/assets/63009846/f209a316-714b-40bc-8704-a3dfbe421551)

## Выводы
Я разработал уровни в известной игре Dragon Picker, которые создаются через Python-скрипт, передаются на гугл таблицу, а от туда уже в Unity

| Plugin | README |
| ------ | ------ |
| Dropbox | [plugins/dropbox/README.md][PlDb] |
| GitHub | [plugins/github/README.md][PlGh] |
| Google Drive | [plugins/googledrive/README.md][PlGd] |
| OneDrive | [plugins/onedrive/README.md][PlOd] |
| Medium | [plugins/medium/README.md][PlMe] |
| Google Analytics | [plugins/googleanalytics/README.md][PlGa] |

## Powered by

**BigDigital Team: Denisov | Fadeev | Panov**
