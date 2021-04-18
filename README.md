<h1>Blurring FoV Restrictor (BFR, 2019)</h1>
<p>해당 페이지에서 Blurring FoV Restrictor 프로젝트의 실험 프로그램을 소개합니다.</p>

<h2>Tech Stack</h2>
<ul>
  <li>Programming Language</li>
  <ul>
    <li><img src="https://img.shields.io/badge/C Sharp-239120?style=flat-square&logo=c-sharp&logoColor=white"/></li>
    <li><img src="https://img.shields.io/badge/Unity Shader Lab-000000?style=flat-square&logo=Unity&logoColor=white"/></li>
  </ul>
  <li>Framework</li>
  <ul>
    <li><img src="https://img.shields.io/badge/Tobii EyeTracker-000000?style=flat-square&logo=Unity&logoColor=white"/></li>
    <li><img src="https://img.shields.io/badge/SteamVR-000000?style=flat-square&logo=Steam&logoColor=white"/></li>
  </ul>
  <li>Toolkit</li>
  <ul>
    <li><img src="https://img.shields.io/badge/Unity-000000?style=flat-square&logo=Unity&logoColor=white"/></li>
    <li><img src="https://img.shields.io/badge/Oculus-1C1E20?style=flat-square&logo=Oculus&logoColor=white"/></li>
    <li><img src="https://img.shields.io/badge/HTC Vive-2e317d?style=flat-square&logo=Steam&logoColor=white"/></li>
  </ul>
</ul>

<h2>Summary</h2>
<p><b>사람의 감정에 따라, 몸 움직임이 다를까?</b> 졸업 논문을 마무리하고, 여유가 되어 진행한 프로젝트입니다. 교내 다른 연구자분과 대학 병원측 교수와 함께 진행했던 과제로 기억합니다. 정확히 어떠한 레퍼런스인지 기억하지 못하지만, 정서 불안이 있는 환자에게 다양한 감정의 분위기를 유도하고 '가장 많이 사용할 신체 부위를 알려주세요'라는 응답을 받았던 것으로 기억합니다. 이를 기반으로, 모션 캡처를 통해서 사람의 동작을 레코딩한 스켈레톤의 부위별 움직임 빈도를 Visualization하는 프로그램을 구현하였습니다. </p>

<h2>Detail</h2>
<p>OptiTrack의 Motive의 Avatar 데이터 Format은 Unity의 Humanoid Avatar Format에 적합합니다. 특히, Avatar 움직임을 부위별로 하이라이팅하기 위해서 스켈레톤의 부위별 Rigging Weight에 따라, Mesh의 rgb 값을 조절하였습니다. 이를테면, 어깨를 돌리는 동작이라면, 어깨 스켈레톤과 관련된 (Weight가 높은) Mesh (e.g. 팔의 상, 하박, 어깨)는 짙은 색상으로 하이라이트됩니다.</p>
<p>위와 같은 원리를 이용해, 메인 하이라이트는 두 가지 방식으로 Visualization 됩니다. 직전 10 프레임 동안의 평균 움직임을 렌더링하는 Realtime Visualization과 애니메이션이 끝난 직후의 총 움직임을 렌더링하는 Stacked Visualization으로 나뉩니다.</p>
<p>또한, 좀 더 자세한 분석을 위해, 스켈레톤 움직임을 Yaw, Pitch, Roll로 구분하여 하이라이트하는 옵션을 추가하였습니다.</p>

<h2>Behind Story</h2>
<p>초기, 긍정적인 정서의 춤과 부정적인 정서의 춤을 찾다가 도저히 원하던 자료를 찾을 수 없었습니다. 해서, 연구실에 취미로 춤 좀 춘다는 후배 석사생을 붙잡아서 도움을 받아냈습니다. 연구실 나오기 직전에, 해당 프로그램은 과제 1년차의 프로토타입이 된다고 들었습니다. 부디 좋은 성과가 있었으면 합니다.</p>

<h2>프로젝트 영상</h2>

[![Unity-MotionCapture](http://img.youtube.com/vi/YxoRnT_WZvE/0.jpg)](http://www.youtube.com/watch?v=YxoRnT_WZvE "AvatarVisualization")




<h1>Blurring FoV Restrictor (BFR, 2019)</h1>
<h3>Unity + Oculus CV1 + Tobii EyeTracker + Vive Pro Eye (Wireless)</h3>
<h4>Dynamic Blurring으로 가상 멀미를 해소시킨다</h4>
<p>석사 과정으로 들어와서, 한국연구재단의 지원을 받아 시도한 연구주제. 
구현은 Unity와 Tobii Eye-tracker Glasses 2를 이용하여 만들었다.
이 방법론은 기존 연구에서 Low Resolution이 가상멀미를 줄일 수 있음을 리포트함에 따라, 전통적인 Field of View Restrictor에 Low Resolution Texture를 추가한 것이다.
구현된 Blurring FoV Restrictor는 총 4가지 조건으로 실험을 구성하였으며, 자세하게는 통제 조건, 움직임에 따른 조절, 움직임과 머리 회전에 따른 조절, 그리고 움직임, 머리, 눈에 따른 조절로 구성된다.</p>
<p>통제 조건과 움직임에 따른 방법에서 가상 멀미 차이를 확인할 수 있었다.
Vive Pro Eye로 옮겨, 후속 실험을 진행하였고, 통제방법과 Gaze Dispersion간 차이가 없음을 확인할 수 있었다. 따라서, 제안된 방법은 사용자의 시야 움직임에 방해되지 않음을 보여준다.</p>
<p>제안된 방법은 VR 사용자의 임장감을 보존하는 동시에, 가상 멀미를 저감하고 사용자의 Visual Searching을 방해하지 않는다는 장점을 지니고 있다.</p>

<h3>프로젝트 영상</h3>

[![Unity VR with Eye-tracker](http://img.youtube.com/vi/ScoWGe8QFSA/0.jpg)](http://www.youtube.com/watch?v=ScoWGe8QFSA "Blurring FoV Restrictor")

<h1>BFR with Various Locomotions (2020)</h1>
<h3>Unity + Oculus CV1 + Tobii EyeTracker + Vive Pro Eye (Wireless)</h3>
<h4>Joystick 뿐만 아니라, 다른 Locomotion에서도 Dynamic Blurring의 멀미 저감 효과를 볼 수 있을까?</h4>
<p>기존 Joystick Locomotion에서 BFR의 멀미 저감 효과를 확인할 수 있었다. 그에 따라, 후속 연구에서는 그 밖의 다양한 Locomotion (Boletsis and Cedergren, 2019)에서도 BFR의 멀미 저감 효과가 있을지 확인하고자 하였다.</p>
<p>참고한 레퍼런스로부터 제시된 Locomotion 종류 중, 가장 많이 연구된 Locomotion을 각각 선정한 내용을 참고하여, Joystick, Teleportation, Walking in Place, 그리고 Real Walking을 선택하여, 구현하였다. 특히, Walking In Place는 고려대 연구팀 (Lee et al, 2018)의 알고리즘을 Replicate하였다.</p>
<i>Ref</i>
<ul>
  <li>Boletsis, C., & Cedergren, J. E. (2019). VR locomotion in the new era of virtual reality: an empirical comparison of prevalent techniques. Advances in Human-Computer Interaction, 2019.</li>
  <li>Lee, J., Ahn, S. C., & Hwang, J. I. (2018). A walking-in-place method for virtual reality using position and orientation tracking. Sensors, 18(9), 2832.
</li>
</ul>

<h3>프로젝트 영상</h3>

[![Unity with VR](http://img.youtube.com/vi/C_SYNg30jQQ/0.jpg)](http://www.youtube.com/watch?v=C_SYNg30jQQ "BFR with Various Locomotions")


