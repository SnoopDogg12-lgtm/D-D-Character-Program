using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Section_1_Character_Creation
{
    public partial class Form1 : Form
    {
        /*
        This method checks the player data
        and determinse if there's existing data we can display
        in the program (e.g. stats, player name etc.)
        */
        void checkInitialData()
        {
            if (new FileInfo(@"\\svr-kn-file01\homedrives$\WS330893\Desktop\DD\D-D-Character-Program-master\Section 1 Character Creation\playerData\playerDetails.txt").Length != 0)
            {
                string jsonString = File.ReadAllText(@"\\svr-kn-file01\homedrives$\WS330893\Desktop\DD\D-D-Character-Program-master\Section 1 Character Creation\playerData\playerDetails.txt");
                PlayerData player = System.Text.Json.JsonSerializer.Deserialize<PlayerData>(jsonString);
                title.Left = 0;
                title.Text = $"Welcome back {player.Name}";
            }
        }

        public Form1()
        {
            InitializeComponent();
            checkInitialData();
            tabControl1.TabPages.Remove(stage2);
            tabControl1.TabPages.Remove(stage3);
            tabControl1.TabPages.Remove(stage4);
            tabControl1.TabPages.Remove(stage5);
        }

        // Creating a player data model
        // which defines the structure of how the
        // player data will be stored
        public class PlayerData
        {
            public string Name { get; set; }
        }

        // Character class which defines the structure for the character
        // data. This will also store data from other classes and objects
        public class CharacterData
        {
            public string race { get; set; }
            public int strengthLevel { get; set; }
            public int charismaLevel { get; set; }
            public int constitutionLevel { get; set; }
            public int darkvisionTraitLevel { get; set; }
            public int dexterityLevel { get; set; }
            public int intelligenceLevel { get; set; }
            public string Class { get; set; }
            public int d { get; set; }
            public string[] savingThrows { get; set; }
            public string[] skills { get; set; }
        }

        // Global values that we must use 
        // throughout the script
        string characterName = "";
        int allowedLenghtForChoice = 0;

        // Submit names button functionality
        /*
        This method takes in player and character name
        and performs actions with that data   
         */
        private void submitNamesButton_Click(object sender, EventArgs e)
        {
            // ------- Player data handilng -------

            string playerName = plNameTextBox.Text;

            // instanciating a player object which will allow us
            // to modify the PlayerData class
            var player = new PlayerData
            {
                Name = playerName
            };

            //Serializing and writing to a text file
            string jsonString = JsonConvert.SerializeObject(player);
            string filePath = @"\\svr-kn-file01\homedrives$\WS330893\Desktop\DD\D-D-Character-Program-master\Section 1 Character Creation\playerData\playerDetails.txt";

            TextWriter tsw = new StreamWriter(filePath);

            //Writing text to the file.
            tsw.WriteLine(jsonString);

            //Close the file.
            tsw.Close();

            MessageBox.Show($"Successfuly created player called {playerName}");
            plNameTextBox.Text = "";

            // ------- Character data handilng -------
            characterName = charNameTextBox.Text;

            //creating character file which will be named whatever the user wishes
            string fileName = @$"\\svr-kn-file01\homedrives$\WS330893\Desktop\DD\D-D-Character-Program-master\Section 1 Character Creation\characters\{characterName}.txt";
            using (FileStream fs = File.Create(fileName))
            {
                Byte[] title = new UTF8Encoding(true).GetBytes(characterName);
            }

            MessageBox.Show($"Successfuly created a character called {characterName}");
            charNameTextBox.Text = "";

            // starting to handle the other data in this case races
            // instanciating a player object which will allow us
            // to modify the PlayerData class

            var characterData = new CharacterData
            {
                race = "",
                strengthLevel = 0,
                charismaLevel = 0,
                constitutionLevel = 0,
                darkvisionTraitLevel = 0,
                dexterityLevel = 0,
                intelligenceLevel = 0,
                Class = "",
                d = 0,
                savingThrows = new string[] { },
                skills = new string[] { }
            };

            //Serializing and writing to a text file
            string charJsonString = JsonConvert.SerializeObject(characterData, Formatting.Indented);
            string charFilePath = @$"\\svr-kn-file01\homedrives$\WS330893\Desktop\DD\D-D-Character-Program-master\Section 1 Character Creation\characters\{characterName}.txt";

            TextWriter charTsw = new StreamWriter(charFilePath, true);

            //Writing text to the file.
            charTsw.WriteLine(charJsonString);

            //Close the file.
            charTsw.Close();

            MessageBox.Show("Go to stage 2 to choose a race for your character!");
            tabControl1.TabPages.Remove(stage1);
            tabControl1.TabPages.Add(stage2);
        }

        /*
                    --------------------
        This section handles the choise of a race, and all of
        the neccesary calculations and also creates the base
        functionality of the data handling system
                    --------------------
         */


        /*
         This method is used to redirect us to another page
         if the half elf race is chosen, since we need to let
         the user to choose what abilities they want to give points to
         */
        void handleMultipleChoice()
        {
            MessageBox.Show("Go to stage 3 to choose your abilities");
            tabControl1.TabPages.Remove(stage2);
            tabControl1.TabPages.Add(stage3);
        }

        /*
        The below method is by far one of the most importan ones
        since this is where the system for caluculating and assigning points
        is based.
        */
        void data(string race, string ability, int increaseValue)
        {
            //array values
            List<int> currentValues = new List<int>();
            List<string> keyValues = new List<string>();

            // json file and string based on it
            string jsonFile = @$"\\svr-kn-file01\homedrives$\WS330893\Desktop\DD\D-D-Character-Program-master\Section 1 Character Creation\characters\{characterName}.txt";
            string jsonString = File.ReadAllText(jsonFile);
            //rss object
            JObject rss = JObject.Parse(jsonString);

            //grab current text file values and store them in memory
            CharacterData character = System.Text.Json.JsonSerializer.Deserialize<CharacterData>(jsonString);
            currentValues.Add(character.strengthLevel);
            currentValues.Add(character.charismaLevel);
            currentValues.Add(character.constitutionLevel);
            currentValues.Add(character.darkvisionTraitLevel);
            currentValues.Add(character.dexterityLevel);
            currentValues.Add(character.intelligenceLevel);

            //store json key values
            keyValues.Add("strengthLevel");
            keyValues.Add("charismaLevel");
            keyValues.Add("constitutionLevel");
            keyValues.Add("darkvisionTraitLevel");
            keyValues.Add("dexterityLevel");
            keyValues.Add("intelligenceLevel");

            int keyIdx = keyValues.IndexOf(ability);

            rss["race"] = race;
            rss[ability] = currentValues[keyIdx] += increaseValue;
            string updatedJsonString = rss.ToString();
            File.WriteAllText(jsonFile, updatedJsonString);

            //MessageBox.Show(currentValues[keyIdx].ToString());
        }

        void giveUpdateToUser()
        {
            MessageBox.Show($"Successfuly updated character race");
            MessageBox.Show($"Go to the next stage to choose a class for your character");
            tabControl1.TabPages.Remove(stage2);
            tabControl1.TabPages.Add(stage4);
        }

        //Here we simply check which race is chosen and we do the
        //particular calculations
        private void submitRaceBut_Click(object sender, EventArgs e)
        {
            bool dragonBornIsChecked = dragonbornBut.Checked;
            bool dwarfIsChecked = dwarfBut.Checked;
            bool elfIsChecked = elfBut.Checked;
            bool gnomeIsChecked = gnomeBut.Checked;
            bool halfElfIsChecked = halfElfBut.Checked;
            bool halflingIsChecked = halflingBut.Checked;
            bool halfOrcIsChecked = halfOrcBut.Checked;
            bool humanIsChecked = humanBut.Checked;
            bool tieflingIsChecked = tieflingBut.Checked;

            if (dragonBornIsChecked)
            {
                data("dragonBorn", "strengthLevel", 2);
                data("dragonBorn", "charismaLevel", 1);
                giveUpdateToUser();

            }
            if (dwarfIsChecked)
            {
                data("dwarf", "constitutionLevel", 2);
                data("dwarf", "darkvisionTraitLevel", 1);
                giveUpdateToUser();
            }
            if (elfIsChecked)
            {
                data("elf", "dexterityLevel", 2);
                data("elf", "darkvisionTraitLevel", 1);
                giveUpdateToUser();
            }
            if (gnomeIsChecked)
            {
                data("gnome", "intelligenceLevel", 2);
                data("gnome", "darkvisionTraitLevel", 1);
                giveUpdateToUser();

            }
            if (halfElfIsChecked)
            {
                handleMultipleChoice();
            }
            if (halflingIsChecked)
            {
                data("halfling", "dexterityLevel", 2);
                giveUpdateToUser();
            }
            if (halfOrcIsChecked)
            {
                data("halforc", "strengthLevel", 2);
                data("halforc", "constitutionLevel", 1);
                data("halforc", "darkvisionTraitLevel", 1);
                giveUpdateToUser();
            }
            if (humanIsChecked)
            {
                data("human", "strengthLevel", 1);
                data("human", "charismaLevel", 1);
                data("human", "constitutionLevel", 1);
                data("human", "darkvisionTraitLevel", 1);
                data("human", "dexterityLevel", 1);
                data("human", "intelligenceLevel", 1);
                giveUpdateToUser();
            }
            if (tieflingIsChecked)
            {
                data("tiefling", "charismaLevel", 2);
                data("tiefling", "intelligenceLevel", 1);
                data("tiefling", "darkvisionTraitLevel", 1);
                giveUpdateToUser();
            }
        }

        /*
        The following method will allow us to handle the scenario
        where the user needs to select which abilities they want to
        give points to
        */
        private void submitAbilitiesBut_Click(object sender, EventArgs e)
        {
            List<string> abilities = new List<string>();

            foreach (var checkBox in stage3.Controls.OfType<CheckBox>())
            {
                if (checkBox.Checked)
                {
                    abilities.Add(checkBox.Tag.ToString());
                }
            }
            // checking if the user selected more than two abilites
            // and if they did, we perform some error checking to
            // prevent them from doing it
            if (abilities.Count == 2)
            {
                //MessageBox.Show(abilities[0]);
                //MessageBox.Show(abilities[1]);
                data("halfElf", "charismaLevel", 2);
                data("halfElf", abilities[0], 1);
                data("halfElf", abilities[1], 1);
                data("halfElf", "darkvisionTraitLevel", 1);

                MessageBox.Show("Successfuly added ablity scores");
                MessageBox.Show("Move on to stage 4 to choose a class for your character");
                tabControl1.TabPages.Remove(stage3);
                tabControl1.TabPages.Add(stage4);
            }
            else if (abilities.Count > 2)
            {
                MessageBox.Show("Cannot select more than two abilities!");
                foreach (var checkedBox2 in stage3.Controls.OfType<CheckBox>())
                {
                    checkedBox2.Checked = false;
                }
            }
            else if (abilities.Count == 1)
            {
                DialogResult result = MessageBox.Show("Are you sure you want only one ability ?", "Warning",
                MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    data("halfElf", "charismaLevel", 2);
                    data("halfElf", abilities[0], 1);
                    data("halfElf", "darkvisionTraitLevel", 1);

                    MessageBox.Show("Successfuly added ablity scores");
                    MessageBox.Show("Move on to stage 4 to choose a class for your character");
                    tabControl1.TabPages.Remove(stage3);
                    tabControl1.TabPages.Add(stage4);
                }
                else if (result == DialogResult.No)
                {
                    foreach (var checkedBox2 in stage3.Controls.OfType<CheckBox>())
                    {
                        checkedBox2.Checked = false;
                    }
                }
            }
            else if (abilities.Count == 0)
            {
                MessageBox.Show("You need to select at least one ability!");

            }
        }

        /*
                          -------------------
       This section is concerned with the choise of a class for
       the character and the neccessary calculations and data handlings
                          ------------------
        */
        void updateDandThrowVals(int dValue, string throwValue1, string throwValue2, string Class)
        {
            List<int> currentDValue = new List<int>();
            // json file and string based on it
            string jsonFile = @$"\\svr-kn-file01\homedrives$\WS330893\Desktop\DD\D-D-Character-Program-master\Section 1 Character Creation\characters\{characterName}.txt";
            string jsonString = File.ReadAllText(jsonFile);
            //rss object
            JObject rss = JObject.Parse(jsonString);
            //grab current text file values and store them in memory
            CharacterData character = System.Text.Json.JsonSerializer.Deserialize<CharacterData>(jsonString);

            //handle the change for the the d value
            currentDValue.Add(character.d);
            rss["d"] = currentDValue[0] += dValue;
            rss["Class"] = Class;

            //handle the change for saving throws values
            ((JArray)rss["savingThrows"]).Add(throwValue1);
            ((JArray)rss["savingThrows"]).Add(throwValue2);

            string updatedJsonString = rss.ToString();
            File.WriteAllText(jsonFile, updatedJsonString);
        }

        void handleSkillsChoice(string Class)
        {   
            //classes array
            List<string> classes = new List<string>();
            classes.Add("barbarian");
            classes.Add("bard");
            classes.Add("cleric");
            classes.Add("druid");
            classes.Add("fighter");
            classes.Add("monk");
            classes.Add("paladin");
            classes.Add("ranger");
            classes.Add("rogue");
            classes.Add("sorcerer");
            classes.Add("warlock");
            classes.Add("wizard");
            //2d array which will hold array of skills
            List<List<string>> skills2D = new List<List<string>>();

            //skills arrays
            string[] skills1 = new string[] { "animalHandling", "athletics", "intimidation", "nature", "perception", "survival" };

            string[] skills2 = new string[] { "acrobatics", "animalHandling", "arcana", "athletics", "deception", "history",
            "insight","intimidation","investigation","medicine","nature","perception","performance","persuasion","religion",
            "sleightOfHand","stealth","survival"};

            string[] skills3 = new string[] { "history","insight","medicine","persuation","religion" };
            string[] skills4 = new string[] {"arcana","animalHandling","insight","medicine","nature","perception","religion",
            "survival"};

            string[] skills5 = new string[] {"acrobatics","animalHandling","athletics","history","insight","intimidation","perception",
            "survival"};
            string[] skills6 = new string[] {"acrobatics","athletics","history","insight","religion","stealth" };

            string[] skills7 = new string[] { "athletics","insight","intimidation","medicine","persuation","religion" };
            string[] skills8 = new string[] {"animalHandling","athletics","isnight","investigation","nature","perception","stealth",
            "survival"};

            string[] skills9 = new string[] {"acrobatics","athletics","deception","insight","intimidation","investigation","perception",
            "performance","persuation","sleightOfHand","stealth"};
            string[] skills10 = new string[] {"arcana","deception","insight","intimidation","persuation","religion"};

            string[] skills11 = new string[] {"arcana","deception","history","intimidation","investigation","nature","religion" };
            string[] skills12 = new string[] {"arcana","history","insight","investigation","medicine","religion" };

            //adding the arrays to the 2d list
            skills2D.Add(skills1.ToList());
            skills2D.Add(skills2.ToList());
            skills2D.Add(skills3.ToList());
            skills2D.Add(skills4.ToList());
            skills2D.Add(skills5.ToList());
            skills2D.Add(skills6.ToList());
            skills2D.Add(skills7.ToList());
            skills2D.Add(skills8.ToList());
            skills2D.Add(skills9.ToList());
            skills2D.Add(skills10.ToList());
            skills2D.Add(skills11.ToList());
            skills2D.Add(skills12.ToList());

            //getting index of class
            int classIdx = classes.IndexOf(Class);
            List<string> currentList = skills2D[classIdx];

            /*
            Sovling the issue with the check boxes not fitting on
            the screen
            */
            string[] skillsFirstEight = new string[] { "acrobatics", "animalHandling", "arcana", "athletics", "deception", "history",
            "insight","intimidation","investigation"};

            string[] skilsSecondEight = new string[] {"medicine","nature","perception","performance","persuasion","religion",
            "sleightOfHand","stealth","survival"};

            List<string> skillsFirstEightList = skillsFirstEight.ToList();
            List<string> skillsSecondEightList = skilsSecondEight.ToList();

            if (Class == "bard")
            {
                //generating the first half of checkboxes
                for (int i = 0; i < skillsFirstEightList.Count; i++)
                {
                    CheckBox checkBox1 = new CheckBox();
                    checkBox1.Text = skillsFirstEightList[i];

                    float currentSize = checkBox1.Font.Size;
                    currentSize -= 0.5F;
                    checkBox1.Font = new Font(checkBox1.Font.Name, currentSize,
                    checkBox1.Font.Style, checkBox1.Font.Unit);

                    checkBox1.Location = new Point(256, 0 + i * 90);
                    checkBox1.Size = new Size(135, 135);
                    checkBox1.ForeColor = SystemColors.Control;
                    stage5.Controls.Add(checkBox1);
                }
                //generating the second half of checkboxes
                for (int i = 0; i < skillsSecondEightList.Count; i++)
                {
                    CheckBox checkBox1 = new CheckBox();
                    checkBox1.Text = skillsSecondEightList[i];

                    float currentSize = checkBox1.Font.Size;
                    currentSize -= 1.0F;
                    checkBox1.Font = new Font(checkBox1.Font.Name, currentSize,
                    checkBox1.Font.Style, checkBox1.Font.Unit);

                    checkBox1.Location = new Point(406, 0 + i * 90);
                    checkBox1.Size = new Size(135, 135);
                    checkBox1.ForeColor = SystemColors.Control;
                    stage5.Controls.Add(checkBox1);
                }
            }
            else if(Class != "bard")
            {
                //generating the checkboxes
                for (int i = 0; i < currentList.Count; i++)
                {
                    CheckBox checkBox1 = new CheckBox();
                    checkBox1.Text = skills2D[classIdx][i];

                    float currentSize = checkBox1.Font.Size;
                    currentSize -= 0.5F;
                    checkBox1.Font = new Font(checkBox1.Font.Name, currentSize,
                    checkBox1.Font.Style, checkBox1.Font.Unit);

                    checkBox1.Location = new Point(256, 10 + i * 80);
                    checkBox1.Size = new Size(120, 120);
                    checkBox1.ForeColor = SystemColors.Control;
                    stage5.Controls.Add(checkBox1);
                }
            }
        }

        void updateSkillsChoiceData(int allowedLenght)
        {
            // json file and string based on it
            string jsonFile = @$"\\svr-kn-file01\homedrives$\WS330893\Desktop\DD\D-D-Character-Program-master\Section 1 Character Creation\characters\{characterName}.txt";
            string jsonString = File.ReadAllText(jsonFile);
            /* rss object which will allow us to pull and edit data from our charcter text file */
            JObject rss = JObject.Parse(jsonString);
            List<string> skills = new List<string>();

            foreach (var checkBox in stage5.Controls.OfType<CheckBox>())
            {
                if (checkBox.Checked)
                {
                    skills.Add(checkBox.Text.ToString());
                }
            }

            /*
            Checking if the user has selected more than the allowed lenght of skills.
            We decide what the allowed lenght is based on the class that has been selected
            */
            if(skills.Count > allowedLenght)
            {
                MessageBox.Show($"Cannot select more than {allowedLenght} skills!");
            }
            else if(skills.Count == allowedLenght)
            {
                for (int i = 0; i < skills.Count; i++)
                {
                    ((JArray)rss["skills"]).Add(skills[i]);
                }
                string updatedJsonString = rss.ToString();
                File.WriteAllText(jsonFile, updatedJsonString);
                MessageBox.Show("Sucessfuly updated character skills!");
            }
        }

        private void submitClassBut_Click(object sender, EventArgs e)
        {
            if (barbarianBut.Checked)
            {
                updateDandThrowVals(12, "Strength", "Constitution","barbarian");
                handleSkillsChoice("barbarian");
                allowedLenghtForChoice = 2;
            }
            if (bardBut.Checked)
            {
                updateDandThrowVals(8, "Dexterity", "Charisma","bard");
                handleSkillsChoice("bard");
                allowedLenghtForChoice = 3;
            }
            if (clericBut.Checked)
            {
                updateDandThrowVals(8, "Wisdom", "Charisma", "cleric");
                handleSkillsChoice("cleric");
                allowedLenghtForChoice = 2;
            }
            if (druidBut.Checked)
            {
                updateDandThrowVals(8, "Intelligence", "Wisdom", "druid");
                handleSkillsChoice("druid");
                allowedLenghtForChoice = 2;
            }
            if (fighterBut.Checked)
            {
                updateDandThrowVals(10, "Strength", "Constitution", "fighter");
                handleSkillsChoice("fighter");
                allowedLenghtForChoice = 2;
            }
            if (monkBut.Checked)
            {
                updateDandThrowVals(8, "Strength", "Dexterity", "monk");
                handleSkillsChoice("monk");
                allowedLenghtForChoice = 2;
            }
            if (paladinBut.Checked)
            {
                updateDandThrowVals(10, "Wisdom", "Charisma", "paladin");
                handleSkillsChoice("paladin");
                allowedLenghtForChoice = 2;
            }
            if (rangerBut.Checked)
            {
                updateDandThrowVals(10, "Strength", "Dexterity", "ranger");
                handleSkillsChoice("ranger");
                allowedLenghtForChoice = 3;
            }
            if (rogueBut.Checked)
            {
                updateDandThrowVals(8, "Dexterity", "Intelligence", "rogue");
                handleSkillsChoice("rogue");
                allowedLenghtForChoice = 4;
            }
            if (sorcererBut.Checked)
            {
                updateDandThrowVals(6, "Constitution", "Charisma", "sorcerer");
                handleSkillsChoice("sorcerer");
                allowedLenghtForChoice = 2;
            }
            if (warlcokBut.Checked)
            {
                updateDandThrowVals(8, "Wisdom", "Charisma", "warlock");
                handleSkillsChoice("warlock");
                allowedLenghtForChoice = 2;
            }
            if (wizardBut.Checked)
            {
                updateDandThrowVals(6, "Intelligence", "Wisdom", "wizard");
                handleSkillsChoice("wizard");
                allowedLenghtForChoice = 2;
            }
            MessageBox.Show("Sucessfuly updated character class!");
            MessageBox.Show("Proceed to the next stage to choose your character skills");
            tabControl1.TabPages.Remove(stage4);
            tabControl1.TabPages.Add(stage5);
        }

        private void submitSkillsBut_Click(object sender, EventArgs e)
        {
            updateSkillsChoiceData(allowedLenghtForChoice);
        }
    }
}
