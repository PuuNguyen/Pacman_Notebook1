**I. GAME PLAY:**

### **1\. Cơ Chế Gameplay Chung (Core Mechanics):**

Mọi level đều chia sẻ các quy tắc cơ bản sau (được quản lý thông qua `PlayScene` và `GameStats`):

* Gameplay sẽ theo cơ chế vượt màn được vận hành theo dạng **Progression (Tiến trình qua màn)**.  
* **Điều kiện qua màn (Win Condition):** Ở mỗi level, người chơi phải hoàn thành nhiệm vụ đặc trưng của màn đó (Ví dụ: Level 1 ăn hết chấm, Level 2 chạy đến cổng thoát trước khi hết giờ, Level 3 sống sót qua sương mù, Level 4 tiêu diệt hết bầy ma trong Power Mode, Level 5 nhặt đủ chìa khóa mở cổng).   
* **Chuyển Level:** Khi hoàn thành mục tiêu, màn hình sẽ hiển thị thông báo chiến thắng (*"Victory\!"* hoặc *"Level Cleared\!"*), cộng điểm thưởng dựa trên thời gian/mạng còn lại, sau đó tự động load dữ liệu và chuyển sang level tiếp theo (từ Level 1 $\rightarrow$ Level 2 $\rightarrow$ Level 3 $\rightarrow$ Level 4 $\rightarrow$ Level 5).  
* **Phá đảo (Game Clear):** Khi vượt qua thành công cả Level 5, người chơi sẽ chính thức phá đảo game, điểm số tổng kết sẽ được lưu vào bảng xếp hạng (Scoreboard).  
* Khi **Player thua cuộc**: Hiển thị thông báo thua cuộc cho người chơi (Ví dụ: *"Game Over\! Bạn đã hết mạng"* hoặc *"Thua cuộc\! Hết thời gian"*). Hiển thị tổng kết ngắn gọn của ván chơi đó: Tổng điểm số đạt được (`Score`), cấp độ màn chơi cuối cùng mà người chơi đã đi đến (`Max Level Reached`), và thời gian đã chơi. Sau đó data sẽ được lưu vào database, những data này sẽ được hiển thị bên ScoreboardForm.  
  \=\> Hiện lên 1 form cung cấp các nút bấm/lựa chọn để người chơi quyết định bước tiếp theo:   
+ **Chơi lại (Play Again / Retry):** Reset lại số mạng, điểm số và đưa người chơi về lại Level 1 (hoặc chơi lại màn vừa thua).  
+ **Về Menu chính (Back to Menu):** Đóng màn hình chơi game hiện tại và đưa người chơi quay trở lại `HomeForm` (`MenuScene`).  
+ **Thoát game (Exit):** Đóng hoàn toàn ứng dụng nếu người chơi không muốn tiếp tục.


* **Hệ thống Lưới (Grid-based movement)**: Bản đồ được chia thành các ô vuông (`TileSize`). Pacman và các con ma di chuyển theo các hướng Lên, Xuống, Trái, Phải theo từng ô vuông và bị chặn lại bởi các bức tường (`#`).  
* **Hệ thống Điểm số & Mạng sống**: Ăn chấm nhỏ (`.`) cộng điểm, ăn siêu viên (`S`) kích hoạt trạng thái phản công (Power Mode). Khi Pacman kích hoạt Power Mode, toàn bộ các con ma trên bản đồ sẽ không biến mất ngay lập tức, mà chúng sẽ chuyển sang trạng thái hoảng sợ (tốc độ di chuyển bị giảm chậm,  hướng di chuyển trở nên ngẫu nhiên, …).  Khi Pacman chạm vào con ma, thì pacman được heal 1 mạng và con ma sẽ bay về spawn or revive ở 1 địa điểm ngẫu nhiên.  
* Chạm vào ma khi không bật chế độ siêu viên sẽ bị trừ 1 mạng (`Lives`), 1 Pacman có tối đa 3 \- 5 mạng.   
* **Điều khiển**: Sử dụng `InputManager` để bắt sự kiện phím bấm từ người chơi theo thời gian thực (Real-time input).  
* **Trí tuệ nhân tạo (Smart AI):** thuật toán tìm đường (như BFS hoặc A\* trong `MathHelper`) để truy đuổi Pacman một cách thông minh và bám sát đường đi ngắn nhất.


**2\. Thiết Kế Chi Tiết Gameplay:**

+ Gameplay có thể có từ 5 level hoặc nhiều hơn.   
+ Mỗi level sẽ có từ 5 Ghost trở lên   
+ Pacman sẽ có 3 mạng mỗi lần chơi. ( Tức là qua 1 level thì pacman sẽ được heal full mạng ). 

\-   **Level 1:** Classic Pacman (Màn Cơ Bản): ( 2 con dí theo player, còn lại random )  
     **Mục tiêu:** Ăn toàn bộ các chấm nhỏ (`.`)  trên bản đồ để mở cổng hoặc qua màn.  
     **Luật chơi truyền thống:**

* Ma di chuyển ngẫu nhiên hoặc bám đuổi cơ bản.  
* Xuất hiện các **Siêu viên (`S`)**: Khi Pacman ăn được, các con ma sẽ chuyển sang màu khác (hoảng sợ), di chuyển chậm lại và Pacman có thể ăn luôn con ma đó để cộng thêm điểm lớn.


\-   **Level 2:** Time Attack (Đua Thời Gian)  
     **Mục tiêu:** Tìm đường ngắn nhất thoát khỏi mê cung trước khi hết giờ.  
     **Luật chơi thay đổi:**

* **Đồng hồ đếm ngược:** Người chơi có giới hạn thời gian (ví dụ: **45 giây**). Giao diện hiển thị thanh thời gian giảm dần ở góc màn hình.  
* **Không cần ăn hết chấm:** Bản đồ sẽ có một ô đặc biệt là **Cổng Thoát (`E` \- Exit)**. Nhiệm vụ của Pacman là né ma, tìm đường đi ngắn nhất đến ô `E` trước khi đồng hồ điểm về 0\. Có thể ăn càng nhiều điểm càng tốt.


\-   **Level 3**: Fog of War (Hạn chế tầm nhìn)  
    **Mục tiêu:** Sống sót và hoàn thành màn chơi trong điều kiện tầm nhìn bị che khuất.  
    **Luật chơi thay đổi:**

* **Hiệu ứng Sương mù (Fog of War):** Toàn bộ bản đồ bị phủ màu đen kịt. Chỉ có một vùng sáng nhỏ xung quanh Pacman (bán kính khoảng 2-3 ô) là hiển thị rõ đường đi, tường và các chấm.  
* **Ma ẩn nấp:** Các con ma ẩn trong bóng tối và chỉ lộ diện khi đến gần vùng sáng của Pacman.


\-   **Level 4**: Ghost Hunter  
    **Mục tiêu:** chủ động tiêu diệt ma và ăn toàn bộ các chấm nhỏ (`.`)  trên bản đồ để mở cổng hoặc qua màn. ( Giống level 1 nhưng thêm Item )  
    **Luật chơi thay đổi:**

* Trên bản đồ sẽ rải rác các Siêu viên (Super Ball \- ký hiệu là `S`). Khi Pacman di chuyển vào ô chứa viên `S` này và "ăn" nó , pacman kích hoạt power mode.  
* **Tốc độ x2:** Pacman nhận được một phần thưởng tốc độ (Speed Boost), di chuyển nhanh hơn hẳn so với các màn khác trong vòng 10-12 giây.  
* **Bán kính càn quét:** Không cần phải chạm trực tiếp vào ô của con ma, bất kỳ con ma nào đi lại gần Pacman trong phạm vi **1 ô xung quanh** (trên, dưới, trái, phải) cũng sẽ bị hút vào và ngủm ngay lập tức. Với mỗi lần kill ghost thì pacman sẽ được heal 1 mạng.  
* Điểm số sẽ nhân cấp số nhân cực lớn nếu bạn ăn liền tù tì nhiều con ma trong thời gian Power Mode (Ví dụ: Con 1 \= 200đ, Con 2 \= 400đ, Con 3 \= 800đ, Con 4 \= 1600đ, Con 5 \= 3200đ).  
* Những con ma bị ăn sẽ không biến mất vĩnh viễn, mà sau khoảng 5 giây sẽ respawn và nó được buff. 


\-   **Level 5:** Labyrinth Boss & Smart AI 

    **Mục tiêu:** Thu thập đủ chìa khóa mở cổng để thắng.

    **Luật chơi thay đổi:**

* Cơ bản là giống level 4 \+ 1  
* **Hệ thống Chìa Khóa (`K` \- Key) & Cổng Khóa (`E` \- Exit):** Bản đồ có giấu 3 chiếc chìa khóa. Pacman bắt buộc phải đi nhặt đủ 3 chìa thì ô Cổng Thoát (`E`) mới mở sáng để qua màn.  
* **Movement của Bot:** ko còn con nào move ngẫu nhiên nữa, và chủ động dí theo player.  
    
    
    
    
  


**II. Cấu trúc Thư mục và Phân chia Lớp (Project Architecture)** 

MyGame/  
├── 📁 MyGame.sln                    
├── 📄 README.md                     
├── 📄 .gitignore                   
└── 📁 src/                          
    └── 📁 MyGame/  
        ├── 📄 MyGame.csproj        
        ├── 📄 Program.cs           
        │  
        ├── 📁 Forms/               
        │   └── 📄 [MainForm.cs](http://MainForm.cs)  
        │    ├── [LoginForm.cs](http://LoginForm.cs)  
        │	   ├── [HomeForm.cs](http://HomeForm.cs)  
        │	   │──  GameForm.cs   
        │     └── ScoreboardForm.cs  
        │	     
        ├── 📁 Core/                
        │   ├── 📄 GameCanvas.cs     
        │   ├── 📄 GameLoop.cs       
        │   └── 📄 InputManager.cs   
        │  
        ├── 📁 Entities/             
        │   ├── 📄 Entity.cs   
        │   ├── 📄 Ghost.cs        
        │   └── 📄 Player.cs         
        │  
        ├── 📁 Scenes/               
        │   ├── 📄 IScene.cs        
        │   ├── 📄 MenuScene.cs      
        │    └── 📄 GameStats.cs     
        │    └── 📄 PlayScene.cs       
         │    
        ├── 📁 Utils/               
        │   └── 📄 MathHelper.cs    
        │  
        └── 📁 Assets/               
         │ ├── 📁 Images/           
         │ └── 📁 Sounds/           
         │  
        └── 📁 Properties/   
	    ├── Resources.resx   
	     └── Resources.Designer.cs 

**III. Chức năng:**

### **📂 Forms/ — Tầng Giao Diện Cửa Sổ (Windows Forms)**

Thư mục này chứa các cửa sổ (Window) độc lập của ứng dụng:

* **MainForm.cs**: Cửa sổ chính chứa khung vẽ game (GameCanvas), đóng vai trò là "Host" (khung bao ngoài) để chứa và hiển thị các Scene bên trong.  
* **LoginForm.cs**: Form đăng nhập hoặc nhập tên người chơi trước khi bước vào thế giới game.  
* **HomeForm.cs**: Form Menu chính của game, chứa các tùy chọn như: Bắt đầu chơi, Chọn Level, Xem Bảng Xếp Hạng, Thoát.  
* **GameForm.cs**: (Tùy chọn phụ) Có thể dùng làm cửa sổ chứa riêng màn chơi game hoặc dùng tích hợp chung với MainForm.  
* **ScoreboardForm.cs**: Form hiển thị bảng xếp hạng điểm số (Top High Scores) của người chơi.

### **📂 Core/ — Tầng Vận Hành Cốt Lõi (Engine Core)**

Trái tim xử lý kỹ thuật giúp game chạy mượt mà, không bị giật lag:

* **GameCanvas.cs**: Bề mặt vẽ (kế thừa từ Panel) đã được bật cơ chế **Double Buffering** (bộ đệm kép) để triệt tiêu hoàn toàn hiện tượng nhấp nháy màn hình khi vẽ liên tục.  
* **GameLoop.cs**: Vòng lặp game chuẩn xác (Fixed-step update \+ render), điều phối nhịp độ game chạy ổn định ở mức cố định (ví dụ 60 khung hình/giây) độc lập với hệ thống Windows Timer.  
* **InputManager.cs**: Bộ quản lý trạng thái bàn phím và chuột, giúp ghi nhận phím bấm của người chơi một cách nhanh nhạy và chính xác nhất.

### **📂 Entities/ — Tầng Đối Tượng Vật Thể**

Nơi định nghĩa các đối tượng có thể tương tác và di chuyển trong game:

* **Entity.cs**: Lớp cơ sở (Base Class) trừu tượng chứa các thuộc tính chung cho mọi vật thể như tọa độ (X, Y), kích thước, hướng di chuyển.  
* **Player.cs**: Lớp đại diện cho người chơi (Pacman), quản lý hướng đi, tốc độ, trạng thái ăn điểm hoặc ăn siêu viên.  
* **Ghost.cs**: Lớp đại diện cho các con ma (Kẻ địch), quản lý trạng thái AI di chuyển ngẫu nhiên hoặc đuổi theo Pacman, trạng thái hoảng sợ.

### **📂 Scenes/ — Tầng Quản Lý Màn Chơi / Trạng Thái**

Mô hình Scene giúp bạn chuyển đổi mượt mà giữa các không gian hiển thị khác nhau mà không cần bật quá nhiều Form rườm rà:

* **IScene.cs**: Giao diện chung (Interface) định nghĩa các phương thức chuẩn cho mọi Scene như: Init() (khởi tạo), Update() (cập nhật logic), Render() (vẽ hình).  
* **MenuScene.cs**: Scene hiển thị màn hình menu chính hoặc giao diện chờ bên trong game.  
* **PlayScene.cs**: Scene chính của trận đấu (Nơi load bản đồ mê cung, quản lý Pacman, các con ma, tính điểm, xử lý va chạm và chuyển level).

### **📂 Utils/ — Tầng Các Hàm Tiện Ích**

* **MathHelper.cs**: Các hàm toán học bổ trợ tính toán khoảng cách (như định lý Pythagoras), tính toán va chạm hình học (AABB collision) hoặc làm tròn tọa độ trên lưới ma trận (Grid-based).

### **📂 Assets/ — Thư mục Tài Nguyên Tĩnh**

Chứa toàn bộ nguyên liệu đồ họa và âm thanh để làm game sinh động:

* **Images/**: Chứa file ảnh sprite, hình Pacman, hình các con ma, hình cục chấm (.), siêu viên (S), tường (\#).  
* **Sounds/**: Chứa file âm thanh hiệu ứng (tiếng ăn chấm, tiếng ăn ma, tiếng thua cuộc) và nhạc nền.

### **📂 Properties/ — Cấu hình hệ thống tự động của C\#**

* **Resources.resx**: Nơi lưu trữ nhúng (embed) các tài nguyên dạng text (như file bản đồ Level1.txt, Level2.txt...) hoặc hình ảnh, âm thanh trực tiếp vào file thực thi.  
* **Resources.Designer.cs**: File tự động sinh ra bởi Visual Studio giúp bạn gọi các tài nguyên trong Resources.resx ra code một cách an toàn kiểu dữ liệu (Strongly-typed).

**IV. Trình tự triển khai chi tiết từng bước:** 

#### **Bước 1: Dựng khung Project & Các lớp nền tảng (`Entities` & `Core`)**

* Viết 3 file cốt lõi trước:  
  1. `Entities/Entity.cs` (Lớp cha chứa tọa độ X, Y).  
  2. `Entities/Player.cs` (Định nghĩa Pacman).  
  3. `Entities/Ghost.cs` (Định nghĩa 5+ con ma và trạng thái hoảng sợ).  
  4. `Core/GameCanvas.cs` và `Core/GameLoop.cs` (Để đảm bảo game chạy mượt, không giật lag).

#### **Bước 2: Xây dựng Level 1 (`PlayScene.cs` cơ bản)**

* Tạo một file bản đồ dạng text đơn giản (ví dụ: `Level1.txt`) đưa vào `Resources.resx`.  
* Lập trình logic cho **Level 1**:  
  * Đọc bản đồ thành ma trận `Grid`.  
  * Cho Pacman di chuyển bằng bàn phím qua `InputManager`.  
  * Cho 5 con ma di chuyển (2 con đuổi theo Player bằng thuật toán đơn giản, 3 con đi ngẫu nhiên).  
  * Xử lý va chạm: Ăn chấm nhỏ cộng điểm, ăn siêu viên `S` kích hoạt Power Mode (ma đổi màu, Pacman đụng vào được \+1 mạng và ma hồi sinh), chạm ma bình thường thì trừ mạng.

#### **Bước 3: Đóng gói vào Scene và Form chính (`MainForm.cs` & `PlayScene.cs`)**

* Tạo `MainForm.cs` chứa `GameCanvas`.  
* Đưa `PlayScene` vào để hiển thị Level 1 lên màn hình. 

#### **Bước 4: Xây dựng lớp vỏ giao diện (`LoginForm`, `HomeForm`, `ScoreboardForm`)**

* Khi Level 1 chơi được và ko thấy bug, bắt đầu làm các Form khác:  
  * `LoginForm.cs`: Nhập tên người chơi.  
  * `HomeForm.cs`: Menu chính bấm nút "Play" sẽ gọi sang `MainForm` (chạy Level 1).  
  * `ScoreboardForm.cs`: Lưu điểm số vào file JSON và hiển thị lên bảng xếp hạng.

#### **Bước 5: Mở rộng các Level 2, 3, 4, 5**

* Khi toàn bộ hệ thống khung sườn, menu, điểm số đã chạy trơn tru, bắt đầu viết tiếp các logic riêng biệt cho từng màn:  
  * Thêm đồng hồ đếm ngược cho **Level 2** (Time Attack).  
  * Thêm hiệu ứng sương mù `GraphicsPath` cho **Level 3** (Fog of War).  
  * Thêm cơ chế càn quét nhân đôi điểm số cho **Level 4** (Super Power Mode).  
  * Thêm hệ thống chìa khóa `K`, cổng khóa `E` và thuật toán tìm đường thông minh A\*/BFS cho **Level 5** (Boss & Smart AI).

