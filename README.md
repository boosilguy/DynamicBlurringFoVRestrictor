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
<p><b>VR Sickness도 해소하면서 Presence를 유지하는 Restrictor.</b> 석사 첫 번째 연구 주제를 스킵하고, 진행한 두 번째 연구입니다. 연구실에서 사전에 먼저 진행된 Eye-tracking Based FoV Restrictor를 바탕으로 연구가 진행되었습니다. 기존 연구에 의하면, VR에 적용된 Low Resolution의 Area of Interest Effect가 VR Task Performance를 유지시키면서, VR Sickness를 저감할 수 있음을 보여준다고 합니다 (<a href="https://ieeexplore.ieee.org/abstract/document/8618360?casa_token=7XIKJHIgh_AAAAAA:rp1yiae0uQFRLDEISb3SLB9tHq-I4r13ahLdix2YKq_MprlbajdrGSWLh_mCDj727ZVWgq-4Tt4">Nie et al, 2019</a>).</p> 이 점에서 착안하여, Dynamic FoV Restrictor (<a href="https://ieeexplore.ieee.org/abstract/document/7460053/?casa_token=9q7iwoZ6RFwAAAAA:jLBkmf-DvmYYMkP2OZ6dnjylGfWx10gia4EnSpJ-emdUeY7tDnZfi4MIexVrL57gXLI9nEGhXUY">Fernandes and Feiner, 2016</a>)의 Restrictor Texture를 Gaussian Blur로 변경하여, VR 사용자의 Presence를 유지하는 방법론을 구현하였습니다.</p>
<p>실험 조건은 대조 방법, Dynamic BFR with Speed, Dynamic BFR with Speed & Rotation, 그리고 Dynamic BFR with Eye로 구성되었습니다. 

<h2>Detail</h2>
<p>피험자는 Fernandes and Feiner의 가상 환경과 유사한 환경에서 비슷한 Navigation Task를 이행했습니다. 약 100개의 이정표를 따라 길을 찾는 Task였으며, 주변에 산과 나무, 다양한 자연 환경이 비치되어, 피험자가 충분히 Task를 즐기며 수행하도록 유도했습니다. 평균적인 실험 시간은 블록 당 약 10분이었으며, 피험자들에게 멀미를 해소하는 휴식시간을 최소 5분 제공하였습니다.</p>
<p>제안된 네 조건 사이의 결과를 비교하기 위해, ANOVA를 통해, 분석을 진행하였습니다. 그 결과, Dynamic BFR이 VR 체험 시간이 길어질수록, 증가하는 VR Sickness를 유의미하게 저감시키는 것을 확인할 수 있었습니다.</p>
<p>제안된 네 조건 사이의 Presence는 유의미한 차이를 보이지 않았습니다. 따라서, Dynamic BFR은 VR 체험자의 몰입도를 저해하지 않는 방법입니다.</p>
<p>더불어, 조건 사이의 안구 움직임에는 유의미한 차이가 없었습니다. 따라서, Dynamic BFR는 VR 체험자의 Visual Search를 방해하지 않는 방법입니다.</p>

<h2>Behind Story</h2>
<p>첫 실험이라 긴장을 많이 했었습니다만, 생각보다 재미있었습니다. 피험자들의 다양한 반응을 경험할 수 있어서 즐기며 실험에 응했습니다.</p>
<p>본 연구를 통해, 해석의 관점을 향상시킬 수 있었습니다. 실제로, Eye Tracker 기반의 Restrictor가 멀미를 해소하지 못했는데, 그 해석으로 Letancy를 생각해 볼 수 있습니다. 인간의 눈은 매우 예민하여, Eye Tracker의 Letancy로 인해 어지러움을 느껴, Dynamic BFR의 멀미 해소를 오히려 방해할 수 있습니다.</p>

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


