using System;
using System.IO;
using System.Diagnostics;
using System.Threading;

namespace FolderCreator
{
    class Program
    {
        static void Main(string[] args)
        {
            StartProgram();
        }

        static void InfiFolderMode(string folderPath, int numSubfolders)
        {
            try
            {
                // Generate the main folder name based on the current time
                string mainFolderName = "Lol";

                // Create the main folder
                string mainFolderPath = Path.Combine(folderPath, mainFolderName);
                Directory.CreateDirectory(mainFolderPath);

                Console.WriteLine($"New Main Folder Created: {mainFolderName}");

                // Create the subfolders inside the main folder
                string currentFolderPath = mainFolderPath;
                for (int i = 1; i <= numSubfolders; i++)
                {
                    // Generate the subfolder name based on the current index
                    string subfolderName = GenerateFolderName(i);

                    // Create the subfolder inside the current folder
                    string subfolderPath = Path.Combine(currentFolderPath, subfolderName);
                    Directory.CreateDirectory(subfolderPath);

                    Console.WriteLine($"New Subfolder Created: {subfolderName} | Amount: ({i})");

                    // Update the current folder path to the new subfolder
                    currentFolderPath = subfolderPath;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Incounted An Error");
                StartProgram();
            }
            
        }




        static void MassFoldersMode(string folderPath, int numFolders)
        {
            try
            {
                for (int i = 0; i < numFolders; i++)
                {
                    // Generate the folder name based on the current index
                    string folderName = GenerateFolderName(i);

                    // Create the folder
                    string folderPathWithNewFolder = Path.Combine(folderPath, folderName);
                    Directory.CreateDirectory(folderPathWithNewFolder);

                    Console.WriteLine($"New Folder Created: {folderName} | Amount: ({i})");
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Incounted An Error");
                StartProgram();
            }

        }

        static void ComboPackMode(string folderPath, int numMainFolders, int numSubfoldersPerMainFolder)
        {
            try
            {
                for (int i = 0; i < numMainFolders; i++)
                {
                    // Generate the main folder name based on the current index
                    string mainFolderName = GenerateFolderName(i);

                    // Create the main folder
                    string mainFolderPath = Path.Combine(folderPath, mainFolderName);
                    Directory.CreateDirectory(mainFolderPath);

                    Console.WriteLine($"New Main Folder Created: {mainFolderName} | Amount: ({i})");

                    // Create the subfolders inside the main folder
                    for (int j = 1; j <= numSubfoldersPerMainFolder; j++)
                    {
                        // Generate the subfolder name based on the current index
                        string subfolderName = $"sub{j}";

                        // Create the subfolder
                        string subfolderPath = Path.Combine(mainFolderPath, subfolderName);
                        Directory.CreateDirectory(subfolderPath);

                        Console.WriteLine($"New Subfolder Created: {subfolderName} | Amount: ({i})");
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Incounted An Error");
                StartProgram();
            }
            
        }


        static string GenerateFolderName(int index)
        {

                const string alphabet = "abcdefghijklmnopqrstuvwxyz0123456789";
                int alphabetLength = alphabet.Length;

                string folderName = "";

                // Use modulo to determine which letter/number to add to the folder name
                while (index >= 0)
                {
                    int letterIndex = index % alphabetLength;
                    folderName = alphabet[letterIndex] + folderName;

                    index = (index - letterIndex) / alphabetLength - 1;
                }
                
                return folderName;

            
        }

        static void StartProgram()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("gathering computer info...");
            Console.WriteLine($"PC : {Environment.MachineName}");
            Console.WriteLine($"Current User : {Environment.UserName}");
            Console.WriteLine($"OS : {Environment.OSVersion}");
            Console.WriteLine($"Avalible Cores : {Environment.ProcessorCount}");
            
            // Ask the user for the location to create the folders
            Console.Write("Enter the location to create the folders: ");
            string path = Console.ReadLine();

            // Ask the user for the number of folders to create
            Console.Write("Enter the number of folders to create: ");
            int numFolders = int.Parse(Console.ReadLine());

            // Ask the user for the mode to use
            Console.WriteLine("Select a mode:");
            Console.WriteLine("1. InfiFolder Mode (1 main folder and constant subfolders)");
            Console.WriteLine("2. MassFolders Mode (only makes main folders)");
            Console.WriteLine("3. ComboPack Mode (a mix of both main and subfolders)");
            Console.Write("Enter the mode number: ");
            int mode = int.Parse(Console.ReadLine());

            // Create a countdown event to wait for all threads to finish
            CountdownEvent countdown = new CountdownEvent(numFolders);

            // Loop through the specified number of folders
            for (int i = 0; i < numFolders; i++)
            {
                // Create a new thread to create the folder
                int index = i;
                Thread thread = new Thread(() =>
                {

                    string folderPath = path;


                    // Create subfolders based on the selected mode
                    switch (mode)
                    {
                        case 1:
                            InfiFolderMode(folderPath, numFolders);
                            break;
                        case 2:
                            MassFoldersMode(folderPath, numFolders);
                            break;
                        case 3:
                            ComboPackMode(folderPath, numFolders, numFolders);
                            break;
                    }

                    // Signal that the thread has finished
                    countdown.Signal();
                });

                // Start the thread
                thread.Start();
            }

            // Wait for all threads to finish
            countdown.Wait();

            Console.WriteLine("Folders created successfully!");
            Console.ReadLine();
        }
    }
}