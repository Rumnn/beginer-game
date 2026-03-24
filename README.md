# 🎮 Beginner Game - 2D Space Survival

Một game 2D được phát triển với **Unity 6.0.3** - lấy cảm hứng từ gameplay cổ điển **Asteroids**.

---

## 📋 Tổng Quan Game

**Thể loại**: Arcade 2D Survival  
**Mục tiêu**: Tránh các chướng ngại vật càng lâu càng tốt để đạt điểm cao nhất

### Cách Chơi
- 🎯 **Điều khiển**: 
  - Giữ chuột: Nhân vật bay (hoặc chạm trên mobile)
  - Di chuyển chuột: Điều chỉnh hướng bay
- 💨 **Booster**: Lửa tên lửa xuất hiện khi cạnh có lực đẩy
- ⚠️ **Game Over**: Khi va chạm với bất kỳ chướng ngại vật nào
- 📊 **Điểm**: Tính dựa trên thời gian sống sót, có High Score lưu trữ

---

## 🏗️ Cấu Trúc Dự Án

### Assets
```
Assets/
├── Scripts/              # Các script game logic chính
│   ├── PlayerController.cs     # Điều khiển nhân vật, input xử lý
│   ├── Obstacle.cs             # Hành động của chướng ngại vật
│   └── UIManager.cs            # Quản lý UI và điểm số
├── Scenes/
│   ├── Game.unity              # Scene chính của game
│   └── SampleScene.unity        # Scene mẫu
├── Prefabs/               # Các prefab (chưa được sử dụng)
├── Materials/             # Material rendering
├── UI/                    # UI Elements
├── UI Toolkit/            # UI Toolkit files
└── Settings/              # Cài đặt game
```

---

## 🎮 Các Thành Phần Chính

### 1. **PlayerController** (`PlayerController.cs`)
Quản lý logic nhân vật chính

**Tính năng**:
- ✈️ Di chuyển với thrust force
- 🎯 Xoay theo vị trí chuột
- 🔥 Hiệu ứng booster flame khi tăng tốc
- 📊 Tính điểm theo thời gian
- 💥 Xử lý va chạm (Game Over)

**Input System**: Sử dụng **Unity Input System** (New Input)
```csharp
moveForward.IsPressed()      // Giữ chuột
lookPosition.ReadValue<Vector2>()  // Vị trí chuột
```

**Thông số**:
- `thrustForce`: 10 (Lực đẩy)
- `maxSpeed`: 5 (Tốc độ tối đa)
- `scoreMultiplier`: 1 (Nhân số điểm)

---

### 2. **Obstacle** (`Obstacle.cs`)
Quản lý các chướng ngại vật di chuyển

**Tính năng**:
- 🎲 Kích thước ngẫu nhiên (0.5 - 2.0)
- 🚀 Tốc độ ngẫu nhiên (50 - 150)
- 🔄 Quay tự động với torque random
- 💫 Hiệu ứng va chạm (bounce effect)

**Thông số**:
- `minSize` - `maxSize`: 0.5 - 2.0 (Kích thước)
- `minSpeed` - `maxSpeed`: 50 - 150 (Tốc độ)
- `maxSpinSpeed`: 10 (Tốc độ quay)
- `maxAllowedSpeed`: 10 (Giới hạn tốc độ thực)

---

### 3. **UIManager** (`UIManager.cs`)
Quản lý giao diện người dùng

**Tính năng**:
- 📊 Hiển thị điểm số hiện tại
- 🏆 Lưu và hiển thị High Score (PlayerPrefs)
- 🔄 Nút Restart để chơi lại
- 🎮 Singleton pattern để truy cập dễ dàng

**HighScore**:
- Lưu trữ trong `PlayerPrefs` (khóa: `"HighScore"`)
- Tự động cập nhật nếu điểm mới cao hơn

---

## 🛠️ Quá Trình Phát Triển

### Phase 1: Cơ Bản (Setup)
- ✅ Tạo dự án Unity 2D
- ✅ Setup scene chính
- ✅ Tạo PlayerController controller

### Phase 2: Input & Điều Khiển
- ✅ Chuyển từ Input cũ sang Input System
- ✅ Xử lý di chuyển chuột
- ✅ Xoay nhân vật theo vị trí

### Phase 3: Chướng Ngại Vật
- ✅ Tạo Obstacle script
- ✅ Random kích thước và tốc độ
- ✅ Thêm hiệu ứng quay (torque)

### Phase 4: UI & Điểm Số
- ✅ Tạo UIManager
- ✅ Hiển thị điểm thời gian thực
- ✅ Lưu High Score

### Phase 5: Hiệu Ứng & Polish
- ✅ Booster flame animation (flicker effect)
- ✅ Bounce effect khi va chạm
- ✅ Explosion effect khi player chết
- ✅ Game Over screen

---

## 🎨 Gameplay Loop

```
1. [START] - Hiển thị scene, Player ở trung tâm
   ↓
2. [INPUT] - Nhận input từ người chơi
   ↓
3. [UPDATE] - Cập nhật vị trí, điểm, UI
   ↓
4. [COLLISION CHECK]
   ├─ Không va chạm → Quay lại bước 2
   └─ Va chạm → lấy bước 5
   ↓
5. [GAME OVER] - Hiển thị Game Over UI, lưu High Score
   ↓
6. [RESTART] - Nhân nút Restart để quay lại bước 1
```

---

## 🔧 Cơ Chế Chính

### **Điều Khiển Nhân Vật**
```csharp
// Nếu nhấn chuột
Vector2 inputPos = lookPosition.ReadValue<Vector2>();
Vector3 worldPos = Camera.main.ScreenToWorldPoint(inputPos);

// Tính hướng từ player đến chuột
Vector2 direction = ((Vector2)worldPos - (Vector2)transform.position).normalized;

// Xoay và đẩy
transform.up = direction;
rb.AddForce(direction * thrustForce);
```

### **Obstacle Di Chuyển**
```csharp
// Random hướng và tốc độ
Vector2 randomDirection = Random.insideUnitCircle.normalized;
rb.AddForce(randomDirection * randomSpeed);

// Thêm quay ngẫu nhiên
rb.AddTorque(Random.Range(-maxSpinSpeed, maxSpinSpeed));
```

### **Tính Điểm**
```csharp
elapsedTime += Time.deltaTime;
score = Mathf.FloorToInt(elapsedTime * scoreMultiplier);
// Nếu điểm mới > High Score → Cập nhật PlayerPrefs
```

---

## ⚙️ Yêu Cầu

- **Unity Version**: 6.0.3.6f1 trở lên
- **Rendering Pipeline**: Universal Render Pipeline (URP) hoặc Built-in
- **Input System**: New Unity Input System
- **Platform**: Windows, Mac, Linux, WebGL hoặc Mobile

---

## 🎯 Mục Tiêu Cải Thiện Tương Lai

- [ ] Thêm nhiều loại chướng ngại vật khác nhau
- [ ] Cấp độ khó tăng dần (waves)
- [ ] Thêm power-ups (shield, speed boost, etc.)
- [ ] Sound effects và music
- [ ] Particle effects tốt hơn
- [ ] Mobile touch controls tối ưu
- [ ] Leaderboard online
- [ ] Animations cho Player
- [ ] Tutorial scene

---

## 📝 Ghi Chú Kỹ Thuật

### Input System
Dự án sử dụng **New Input System** (Input API mới của Unity) thay vì Input cũ:
```csharp
using UnityEngine.InputSystem;

public InputAction moveForward;    // Chuối/click
public InputAction lookPosition;   // Vị trí chuột/ngón tay
```

### Physics 2D
- Nhân vật sử dụng **Rigidbody2D** với `linearVelocity`
- Obstacles cũng dùng Rigidbody2D với torque
- Collision callbacks: `OnCollisionEnter2D()`

### Singleton Pattern (UIManager)
```csharp
if (instance == null) instance = this;
else Destroy(gameObject);

// Gọi từ nơi khác
UIManager.instance.UpdateScoreUI(score);
```

---

## 🚀 Cách Chạy

1. Mở dự án trong Unity 6.0.3+
2. Mở scene `Assets/Scenes/Game.unity`
3. Nhấn Play
4. Giữ chuột/chạm để bay, tránh obs với chướng ngại vật

---

## 📄 License

Đây là dự án học tập cho người mới bắt đầu với Unity.

---

**Phát triển bởi**: Rumnn  
**Ngày tạo**: 2026  
**Trạng thái**: Đang phát triển 🚀
