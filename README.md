# 正方教务管理系统抢课工具 #

教务管理系统抢课工具 - 自定教务系统地址版

## 配置&使用 ##

如果你是**广东水利电力职业技术学院**的学生，则无需配置**xsxk.exe.config**！

### 配置 ###

1. 用任意文本编辑器打开.config文件

2. 进入学校正方教务系统登陆界面，把登录界面的URL复制下来，粘贴到login的value处，如

   ```
   <add key="login" value="http://jw1.gdsdxy.cn:81/(zs0xzqifbnov3l55guj3ai55)/default2.aspx"/>
   ```

3. 查看登录页源代码，找到__VIEWSTATE的值，复制下来

   ```
   <input type="hidden" name="__VIEWSTATE" value="dDwyOTIzOTAzMDY7Oz4Awh/0AYQFLiv3J2BJ8gOJ+vV+jw==" />
   ```

4. [站长之家 - URL编码/解码工具](http://tool.chinaz.com/tools/urlencode.aspx)，把复制下来的值粘贴进去，点击编码【**只点一次，只点一次，只点一次**】

转码前：`dDwyOTIzOTAzMDY7Oz4Awh/0AYQFLiv3J2BJ8gOJ+vV+jw==`

转码后：`dDwyOTIzOTAzMDY7Oz4Awh%2f0AYQFLiv3J2BJ8gOJ%2bvV%2bjw%3d%3d`

5. 把转码后的__VIEWSTATE的值粘贴到vstate的value处

   ```
   <add key="vstate" value="dDwyOTIzOTAzMDY7Oz4Awh%2f0AYQFLiv3J2BJ8gOJ%2bvV%2bjw%3d%3d"/>
   ```

4. 用你的账号登陆进入正方教务系统后台，网上选课->任选课网上选课->右键->复制链接地址，把内容粘贴到index的value处并删除`?xh=`后面的内容，最后变成

   ```
   <add key="index" value="http://jw1.gdsdxy.cn:81/xf_xsqxxxk.aspx?xh="/>
   ```

### 使用 ###

1. 打开zfxk.exe直接运行，按照提示输入账号密码，选多门课的话课程代码请用英文逗号 `,` 分开，也可以使用下方代码快速地开始。

`xsxk.exe 学号 密码 课程代码`

2. 关于**水院的课程代码**，仔细的人会发现水院的有些课程代码是相同的，但是你可以通过点击课程名称所弹出来的窗口中在`&xkkh=`背后找到像`(2016-2017-2)-01P470432-100522-10`这样的真正的课程代码。【目前还没确定是不是，只能等下次抢课我再抓包试试看吧】

3. 当抢课成功，会提示『选课成功』。 

4. 同时会保存最后抓取到的抢课页面为result.html，可以通过这个页面确认是否已经抢课成功。

6. 可以通过自己编译来开启当抢课成功一次后，将保存课程列表到class.csv的功能。**【水院无效】**

## 注意 ##

1. 强烈不建议多开。

2. 请勿用于商业用途。

3. 原作者在[Issue#2](https://github.com/imlinhanchao/zfxk2/issues/2)中提到**填写多个课号时，可能造成随机选中一门课程。**所以请谨慎使用。

4. 程序**主要适配**广东水院，其他院校请**自行适配**。

5. 程序没有.config也是可以正常使用的，不过默认使用的是水院的参数。

## 更新日志 ##

### 1.04 ###

1. 针对水院教务系统做了大量的适配，等下次抢课我会继续完善。

2. 修复了之前一直没发现的、可能导致程序失效的一个重大Bug。

3. .config增加__VIEWSTATE值，这是影响登陆成功的一个重要参数。

### 1.03 ###

1. 在输入密码处采用星号代替字符以增强安全性。

2. 增加.config可以配置自定义登陆页面和抢课页面。

3. 删除了带参启动。

### 1.02 ###

1. 初始版本，只是把网址改成了水院的。