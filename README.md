# TwinCity

** A simulation for Mixed Reality Digital Twin city! ** 

We are four master students for an academic group project in the course *Design for Complex and Dynamic Contexts (DCDC)* at Stockholm University in Spring 2025. In this project, we develop a collaborative tool, the Mixed Reality Emergency Response application, with a 3D digital twin city displayed on a physical table. Our project aims to allow users to visualize a city and ongoing emergency scenarios in a 3D map, improving their understanding of spatial context. Our project also enables to use of hand tracking in MR to rotate, zoom, and pan the city model naturally, as one would with a physical model. Lastly, our project aims to support multiple co-located users to view and manipulate the same city model together in real-time. Shared Spatial Anchors (SSA) ensure all users see the model in the exact same physical location on the table, and networking (Photo Fusion) syncs their interactions. These solutions help to fill the gaps of ineffective cooperation between different teams whenever a disaster occurs.

# Logo poster

## 1.Introduction

TwinCity is an experimental Mixed Reality (MR) platform that integrates Unity, Cesium for Unity, and Google Photorealistic 3D Tiles to create a real-time 3D digital twin of urban environments. Designed as a collaborative emergency response tool, it enables users to simulate disasters, analyze impacts, and coordinate interventions within a dynamic virtual replica of real-world locations. As climate change increasingly disrupts lives, effective cross-departmental coordination is crucial. Unlike traditional tools, TwinCity enhances situational awareness by allowing teams to explore, collaborate, and analyze data in an immersive 3D space. Users can work together seamlessly to address environmental challenges, gain real-time insights from disaster zones, and optimize response strategies, thereby improving decision-making and operational efficiency.

TwinCity creates a high-fidelity virtual replica of physical cities, enabling stakeholders to visualize, analyze, and interact with geospatial data in an immersive 3D environment. The platform serves as a collaborative decision-support system for government agencies, disaster response teams, and urban planners. By integrating real-time IoT sensor data, environmental simulations, and multi-user interaction, TwinCity facilitates disaster preparedness & response in simulating floods, fires, and other emergencies in a risk-free virtual space. Climate Adaptation planning in modeling the impact of extreme weather events on urban infrastructure. Cross-departmental coordination in enabling real-time collaboration among emergency services, utilities, and policymakers.

## 2.Design process

TwinCity is a Mixed Reality (MR) project that utilizes digital twin technology to simulate water flood scenarios in Solna. The purpose of this project is to join multiple users to interact simultaneously and immersively. We began with brainstorming as the first step of the design process. In this step, we outline the project's scope, objectives, and potential challenges. Then, we defined our user interactions within the MR environment and tested various technologies to identify the most suitable tools for our implementation. All virtual environments in this project were developed by Unity 6, including modeling 3D maps and interactive maps, buttons, and lines. Collaborative features were implemented to allow multiple users to engage with the digital twin simultaneously. Subsequent user testing on demo day provided feedback on usability, functionality, and overall experience, which led to the identification and resolution of technical and usability issues. Refinements were made based on our user feedback and testing results. Finally, the project was compiled into a fully functional presentation, ready to be showcased to the intended audience.

### 2.1. Brainstorming

The brainstorming phase involved generated and refined the whole ideas for our project to establish the objectives, scope and potential challenges. At first, we defined our problem statement, user personas and user journey.

### Problem statement
Rapid urbanization and climate change are increasing the frequency and severity of urban disasters (floods, fires, power outages). Current emergency response systems rely on static 2D maps, siloed data, and slow manual coordination, leading to delayed decisions due to fragmented information and inefficient resource allocation. Today, existing tools fail to address the challenges that disasters bring in our lives, due to a lack of real-time 3D visualization, forcing responders to mentally translate 2D maps into physical spaces. In addition, operation in isolation with emergency teams, city planners, and utilities incompatible systems. Many tools depend on historical data, ignoring live IoT sensor input (e.g., weather, traffic, structural sensors), and also do not offer collaborative simulation, making it impossible to test strategies before implementation.

Stockholm is an urban city that may face increasing risk or disaster due to climate change. The city's dense infrastructure and limited green spaces hinder effective water runoff, leading to potential damage to property, infrastructure, and public safety.

### Purpose
Develop an interactive Mixed Reality (MR) tool that enables different teams to collaboratively visualize, analyze, and strategize flood mitigation by using historical and predictive data.

### 

Our project continue with discuss the existed organized ideas into different stages and corresponding modules.  


Finally, we will showcase the technical capabilities to a general audience in a video format.
and present how different teams can work effectively, providing smart solutions for responders in the event of a disaster or emergency.
## 3. System description

Features and fuctionalities

## 4.Installation

- **Unity 6 LTS**
- **Cesium for Unity**
- **Google Maps 3D Tiles API**
- **Meta XR SDK** (Passthrough + Hand Tracking)
- **Photon Fusion** (Multiplayer Framework)
- **Visual Studio Code + GitHub**

---

## 5.Usage section

- ✅ 3D photorealistic Digital Twin city rendered in Unity  
- ✅ Flood simulation using water plane or visual overlays  
- ✅ MR interface with gesture-based navigation  
- 🔄 Multiplayer interaction for emergency response roles  
- 🔄 Visual indicators and alerts for dynamic events  
- 🔄 Subscene placement and collision-aware environment

---

## 6.How to Run

> Requires Unity 6 LTS and Cesium for Unity setup.

1. Clone the repo: ```git clone https://github.com/<your-username>/twincity.git```
2. Open in **Unity 6**
3. Sign in to your **Cesium account**
4. Enter your **Google Maps API key** to load 3D tiles
5. Enter Play Mode

---

## 👥 Contributors

- **Andreas** – Backend, Unity architecture, repo management  
- **Eman** – UI/UX design, Figma mockups, portfolio materials
- **Florian** – Lead Unity implementation, Cesium integration  
- **Minhui** – Interaction design, XR developer, Testing

---

## 📁 License

This project is open source under the [MIT License](LICENSE).

---

## 💡 Acknowledgments

- Cesium for Unity: [https://cesium.com/unity](https://cesium.com/unity)  
- Google Maps Photorealistic Tiles  
- Original concept inspiration: [YouTube Video](https://www.youtube.com/watch?v=lLw5hCqSv5Y)
