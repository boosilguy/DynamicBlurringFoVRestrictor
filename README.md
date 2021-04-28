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
![BFR](https://user-images.githubusercontent.com/30020288/115150139-11e07180-a0a2-11eb-8e0c-5e110b392ea6.PNG)
<p>피험자는 Fernandes and Feiner의 가상 환경과 유사한 환경에서 비슷한 Navigation Task를 이행했습니다. 약 100개의 이정표를 따라 길을 찾는 Task였으며, 주변에 산과 나무, 다양한 자연 환경이 비치되어, 피험자가 충분히 Task를 즐기며 수행하도록 유도했습니다. 평균적인 실험 시간은 블록 당 약 10분이었으며, 피험자들에게 멀미를 해소하는 휴식시간을 최소 5분 제공하였습니다.</p>
<p>제안된 네 조건 사이의 결과를 비교하기 위해, ANOVA를 통해, 분석을 진행하였습니다. 그 결과, Dynamic BFR이 VR 체험 시간이 길어질수록, 증가하는 VR Sickness를 유의미하게 저감시키는 것을 확인할 수 있었습니다.</p>
<p>제안된 네 조건 사이의 Presence는 유의미한 차이를 보이지 않았습니다. 따라서, Dynamic BFR은 VR 체험자의 몰입도를 저해하지 않는 방법입니다.</p>
<p>더불어, 조건 사이의 안구 움직임에는 유의미한 차이가 없었습니다. 따라서, Dynamic BFR는 VR 체험자의 Visual Search를 방해하지 않는 방법입니다.</p>

![BFR FoV Restrictor](https://user-images.githubusercontent.com/30020288/116349620-5fd84080-a82b-11eb-9583-a9bf023227ee.png)
<p>Unity Shader로 Rendering된 Camera View에 마스크를 입혔습니다. 수평 방향으로 Dynamic Gaussian Filter Size에 따라 계산이 선행되었으며, 조건에 따라 사용자 움직임으로 조절되는 FoV 사이즈 만큼, Gaussian Filter가 적용되지 않을 부분의 Opacity를 낮추었습니다. 다음 Shader Pass에서, 수직 방향으로도 동일하게 계산되도록 코드를 구현하였습니다.</p>

![BFR Sudo](https://user-images.githubusercontent.com/30020288/116349617-5cdd5000-a82b-11eb-83b1-4667bfab6c07.png)

<h2>Behind Story</h2>
<p>첫 실험이라 긴장을 많이 했었습니다만, 생각보다 재미있었습니다. 피험자들의 다양한 반응을 경험할 수 있어서 즐기며 실험에 응했습니다.</p>
<p>본 연구를 통해, 해석의 관점을 향상시킬 수 있었습니다. 실제로, Eye Tracker 기반의 Restrictor가 멀미를 해소하지 못했는데, 그 해석으로 Letancy를 생각해 볼 수 있습니다. 인간의 눈은 매우 예민하여, Eye Tracker의 Letancy로 인해 어지러움을 느껴, Dynamic BFR의 멀미 해소를 오히려 방해할 수 있습니다.</p>

<h2>Check This!</h2>
<a href="http://www.riss.kr/search/detail/DetailView.do?p_mat_type=be54d9b8bc7cdb09&control_no=aacc914c7d9694e5ffe0bdc3ef48d419">Thesis</a>

<h2>실행 영상</h2>

[![BFR! (Unity VR App)](http://img.youtube.com/vi/ScoWGe8QFSA/0.jpg)](http://www.youtube.com/watch?v=ScoWGe8QFSA "BFR!")
