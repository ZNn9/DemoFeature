- Đối chiếu Version của Game hiện tại của player với Version của Game trên Cloud Server - Check
    + ...
- Sau khi kết thúc bước đăng ký, đăng nhập. - ĐK/ĐN
- Vào giao diện chính
- khi nhấn vào nút chơi game - Play Game
- Hệ thống kiểm tra trạng thái người chơi
    + Nếu tài khoản người dùng mới
        + Hệ thống sẽ "Check" đây là tài khoản <<player trial>> hay <<tài khoản chính quy>> - If Else
            + Nếu là <<tài khoản chính quy>> sẽ được "NewSave" trên Local và Cloud Server - New Save
            + Nếu là <<player trial>> sẽ được "NewSaveInOnlyLocal" trên chỉ Local - New Save In Only Local
                + Nếu người chơi "Liên Kết Tài Khoản"
                    + Dữ liệu người dùng sẽ được "Save" trên Cloud Server
    + Nếu là người cũ hệ thống sẽ "Check" và đối chiếu "idPlayer" với file dữ liệu của người chơi "<<playerData'+'idPlayer>>.json" - Local + Tài Khoản cũ
        + Nếu dữ liệu .json của người dùng không tồn tại hoặc số "idPlayer" của file không tương ứng - If Else
            + Hệ sẽ "Load" dự liệu từ cloud - Cloud Server
                + Sau khi kết thúc trò chơi game sẽ "Save" dự liệu vừa kết thúc của người chơi trên Local - Local
                + Sau khi lưu xong tại Local sẽ "Save" lên trên Cloud (Phải có mạng) - Cloud Server
        + Nếu dữ liêu .json của người dùng tồn tại và số "idPlayer" của file tương ưng - If Else
            + Hệ thống sẽ "Check" và đối chiếu mã version ULID với dữ liệu trên Cloud Server - Check
                + Hệ thống sẽ "Check" version mới nhất thì "load" - Load
                    + Sau khi kết thúc trò chơi game sẽ "Save" dự liệu vừa kết thúc của người chơi trên Local - Local
                    + Sau khi lưu xong tại Local sẽ "Save" lên trên Cloud (Phải có mạng) - Cloud Server
    
