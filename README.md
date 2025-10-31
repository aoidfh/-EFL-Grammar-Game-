# 어린이 영어 학습 게임 (EFL Grammar Game Collection)

Unity(C#) 기반으로 개발한 교육용 영어 학습 게임입니다.  
학습자가 직접 단어를 클릭하며 문법을 익히는 구조로,  
‘재미와 학습 효과의 균형’을 목표로 설계되었습니다.  

총 4개의 미니게임으로 구성되어 있으며,  
3개의 **Fill-in-the-Blank** 게임과 1개의 **English Sentence Completion** 게임을 포함합니다.  

---

## 🎯 프로젝트 개요 (Project Overview)
- **프로젝트 기간:** 2024.03 ~ 2024.06 (4개월, 개인 프로젝트)
- **개발 환경:** Unity (C#), Visual Studio  
- **플랫폼:** PC / Web  
- **장르:** Educational, Puzzle  
- **역할:** 전체 기획 및 프로그래밍, UI 설계, 게임 로직 구현  

---

## 🧩 주요 기능 (Key Features)
- **Fill-in-the-Blank 미니게임 (3종)**  
  제시된 문장에서 빈칸을 맞추는 문법 학습형 게임  
  난이도에 따라 단어 개수 및 문장 구조가 다르게 설정  

- **English Sentence Completion**  
  주어진 단어를 올바른 순서로 클릭하여 문장을 완성하는 UI 로직 구현  

- **JSON 기반 문제 관리 시스템**  
  문제와 정답 데이터를 JSON으로 관리하여 유지보수성과 재사용성 향상  
  엑셀을 통해 문제 세트를 쉽게 수정·확장할 수 있도록 설계  

- **UI 및 피드백 시스템**  
  정답 여부에 따라 색상 및 사운드 피드백 제공  
  CanvasGroup과 Animator를 이용해 부드러운 화면 전환 구현  

---

## ⚙️ 기술 구현 (Implementation Details)
- FSM(상태 기계) 로직을 이용하여  
  `Idle → Check → Feedback` 단계로 상태 전환 구성  
- JSON 파싱을 통한 문제 로드 및 정답 판정 로직 분리  
- CanvasGroup, Animator 기반 UI 전환 최적화  

---

## 📊 결과 및 성과 (Results & Achievements)
- 총 4종의 미니게임 완성 및 동작 검증 완료  
- 학습자의 문법 이해도 및 학습 몰입도 향상  
- JSON 기반 문제 구조를 통해 유지보수성과 확장성 확보  
- FSM 구조 및 UI 전환 구현 경험을 통해 Unity 엔진 활용 능력 강화  
- YouTube Demo를 통해 외부 공유 및 포트폴리오화 완료  

🎥 **Demo Video:** [https://youtu.be/JDJV4qhHXR4](https://youtu.be/JDJV4qhHXR4)

---

## 🧰 기술 스택 (Tech Stack)
- **Language:** C#  
- **Engine:** Unity 2020  
- **Tools:** Visual Studio, PowerDirector  
- **Data Format:** JSON

---

## 📫 Contact
- **Developer:** 박준영 (Junyeong Park)  
- **Email:** soiw5386@naver.com  
