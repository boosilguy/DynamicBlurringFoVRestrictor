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
<h5>Ref</h5>
<ul>
  <li>Boletsis, C., & Cedergren, J. E. (2019). VR locomotion in the new era of virtual reality: an empirical comparison of prevalent techniques. Advances in Human-Computer Interaction, 2019.</li>
  <li>Lee, J., Ahn, S. C., & Hwang, J. I. (2018). A walking-in-place method for virtual reality using position and orientation tracking. Sensors, 18(9), 2832.
</li>
</ul>

<h3>프로젝트 영상</h3>

[![Unity with VR](http://img.youtube.com/vi/C_SYNg30jQQ/0.jpg)](http://www.youtube.com/watch?v=C_SYNg30jQQ "BFR with Various Locomotions")


