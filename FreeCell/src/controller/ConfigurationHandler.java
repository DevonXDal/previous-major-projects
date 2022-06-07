package controller;

import java.io.*;
import java.net.URISyntaxException;
import java.util.HashMap;
import java.util.NoSuchElementException;
import java.util.Scanner;

/**
 * The class ConfigurationHandler handles the reading of writing of configuration information from a file.
 * It will return a boolean response showing whether or not reading or writing were successful.
 *
 * @author Devon X. Dalrymple
 * @version 2020-12-10
 */
public class ConfigurationHandler
{

    private HashMap<String, Integer> config;
    private static final String FILE_NAME = "FreeCell.config";

    /**
     * Default constructor for ConfigurationHandler.
     */
    public ConfigurationHandler()
    {
        config = new HashMap<>();
    }

    /**
     * Getter for the configuration settings retrieved by the ConfigurationHandler
     *
     * @return The config settings
     */
    public HashMap<String, Integer> getConfig()
    {
        return config;
    }

    /**
     * Reads the configuration file if it exists and stores the key-value pairs
     *
     * @return true if the configuration was successfully read, will also turn false if the config is left empty
     */
    public boolean readConfiguration()
    {
        config.clear();

        try (Scanner configReader = new Scanner(new File(findFilePath() + FILE_NAME)))
        {
            while (true)
            {
                try
                {
                    String currLine = configReader.nextLine();

                    String key = null;
                    int pipeFoundAt = currLine.length();
                    for (int i = 0; i < currLine.length(); i++)
                    {
                        if (currLine.charAt(i) == '|')
                        {
                            key = currLine.substring(0, i);
                            pipeFoundAt = (i);
                            break;
                        }
                    }

                    Integer value = null;
                    for (int i = pipeFoundAt + 1; i < currLine.length(); i++)
                    {
                        if (currLine.charAt(i) == ';')
                        {
                            String toTranslate = currLine.substring(pipeFoundAt + 1, i);
                            value = Integer.parseInt(toTranslate);
                            break;
                        }
                    }
                    config.put(key, value);
                }
                catch (NoSuchElementException ignored)
                {
                    break;
                }
            }
        }
        catch (Exception ignored)
        {
            return false;
        }

        if (config.keySet().size() == 0)
        {
            return false;
        }
        return true;
    }

    /**
     * Writes a configuration file if able and stores the given key-value pairs
     *
     * @param settings The key-value pairs to store as a file (No | [Vertical Pipe] or ; [Semicolon] Allowed])
     * @return true if the configuration was successfully written
     */
    public boolean writeConfiguration(HashMap<String, Integer> settings)
    {
        try (PrintWriter out = new PrintWriter(findFilePath() + FILE_NAME))
        {
            for (String key : settings.keySet())
            {
                out.write(key + "|" + settings.get(key) + ";\n");
            }
        }
        catch (IOException | URISyntaxException ignored)
        {
            return false;

        }
        return true;
    }

    /**
     * Alters the set configuration in a file if able and saves the new key value pairs.
     * The configuration file must have a matching key in order to be altered.
     *
     * @param settingsToChange The values to be changed for the specified keys (No | [Vertical Pipe] or ; [Semicolon] Allowed])
     * @return true if the configuration was successfully altered
     */
    public boolean alterConfiguration(HashMap<String, Integer> settingsToChange)
    {
        HashMap<String, Integer> dataToAlter = new HashMap<>();
        try (Scanner configReader = new Scanner(new File(findFilePath() + FILE_NAME))) //Read from the file
        {
            while (true)
            {
                try
                {
                    String currLine = configReader.nextLine();

                    String key = null;
                    int pipeFoundAt = currLine.length();
                    for (int i = 0; i < currLine.length(); i++)
                    {
                        if (currLine.charAt(i) == '|')
                        {
                            key = currLine.substring(0, i);
                            pipeFoundAt = (i);
                            break;
                        }
                    }

                    Integer value = null;
                    for (int i = pipeFoundAt + 1; i < currLine.length(); i++)
                    {
                        if (currLine.charAt(i) == ';')
                        {
                            String toTranslate = currLine.substring(pipeFoundAt + 1, i);
                            value = Integer.parseInt(toTranslate);
                            break;
                        }
                    }
                    dataToAlter.put(key, value);
                }
                catch (NoSuchElementException ignored)
                {
                    break;
                }
            }
        }
        catch (Exception ignored)
        {
            return false;
        }

        for (String keyToBeAltered : settingsToChange.keySet())
        {
            dataToAlter.replace(keyToBeAltered, settingsToChange.get(keyToBeAltered));
        }

        if (!writeConfiguration(dataToAlter))
        {
            return false;
        }

        return true;
    }

    /*
    Grabs the filepath for the config file
    https://stackoverflow.com/questions/320542/how-to-get-the-path-of-a-running-jar-file
     */
    private String findFilePath() throws URISyntaxException
    {
         String path = (ConfigurationHandler.class.getProtectionDomain().getCodeSource().getLocation()
                .toURI()).getPath();

        int currChar = path.length() -1;
        while (currChar >= 0 && path.charAt(currChar) != '/' && path.charAt(currChar) != '\\')
        {
            if (currChar >= path.length())
            {
                currChar--;
            }
            else if (currChar == 0)
            {
                return null;
            }
            else
            {
                path = path.substring(0, (currChar));
                currChar--;
            }
        }
        return path;
    }

}
