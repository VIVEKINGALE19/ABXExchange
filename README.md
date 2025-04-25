This is a C# Console Application that connects to a mock ABX Exchange Server via TCP, requests all stock ticker packets, handles missing sequences, and exports a complete data set to a JSON file.

📦 Requirements
Visual Studio 2022

.NET 6.0 or higher (tested with .NET 6 and .NET 8)

Node.js (v16.17.0 or higher) to run the ABX Exchange Server

🚀 Setup Instructions
1. Clone or Download This Repository
    git clone https://github.com/your-username/abx-exchange-client.git
    cd abx-exchange-client
2. Start the ABX Exchange Server
   Download the mock exchange server from the test prompt (abx_exchange_server.zip).

Unzip the contents.

Open a terminal in that folder and run:
node main.js

Note: Make sure Node.js v16.17.0+ is installed and port 3000 is available.

3. Open the Project in Visual Studio 2022
Open Visual Studio.
Click on “Open a project or solution” and select AbxExchangeClient.sln.

4. Build and Run the Application
Press Ctrl+F5 or click on the “Start Without Debugging” button.

5. Output
After successful execution, the client will fetch and validate packets.

The complete data set will be written to a JSON file named:
output.json

Default location:
Project Folder → output.json

You can change the path in Program.cs or the SaveToJsonAsync method if needed.
