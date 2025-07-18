# InvestigateAR-Game
AR-based Detective Game using Unity, Firebase, and Python (Planned ML Integration)
# 🕵️ Investigate AR – Augmented Reality Detective Game

**Investigate AR** is an interactive mobile detective game using **Augmented Reality**. Players explore virtual crime scenes by placing and interacting with clues overlaid in their real-world environment via smartphone.

> ⚠️ *This project was originally submitted for my MCA program. I am now continuing development independently, integrating planned features like AI-based object detection using YOLOv8.*

---

## 🧩 Tech Stack

| Feature                | Technology                             |
|------------------------|-----------------------------------------|
| Game Engine            | Unity LTS 2022                          |
| Language               | C#                                      |
| Database               | Firebase Realtime DB                    |
| AR Support             | AR Foundation, ARCore                   |
| Dev Tools              | VS Code        |
| ML (Planned)           | Python + YOLOv8 (object detection)      |

---

## 🧠 Features Implemented

- 📍 AR-based clue placement using touch and plane detection
- 🔄 Clue data sync with Firebase database
- 🔍 Clue retrieval and logging from cloud
- 🧪 Planned ML-based smart clue generation (using YOLOv8)

---

## 🔧 Code Sample

```csharp
void PlaceClue(Vector3 position)
{
    GameObject clue = Instantiate(cluePrefab, position, Quaternion.identity);
    clue.SetActive(true);
    SyncClueData(position);
}
