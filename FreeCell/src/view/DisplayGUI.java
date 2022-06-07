package view;

import controller.Controller;
import externalcodenotmine.OverlapLayout;
import org.jetbrains.annotations.Nullable;

import javax.swing.*;
import java.awt.*;
import java.awt.event.*;
import java.io.IOException;
import java.net.URI;
import java.net.URISyntaxException;
import java.util.ArrayList;
import java.util.HashMap;

/**
 * The class DisplayGUI acts as the view for FreeCell Solitaire. It contains the frame that is displayed in addition to the content
 * and actions that are carried out from that frame.
 *
 * @author Devon X. Dalrymple
 * @version 2020-12-10
 */
public class DisplayGUI
{
    //Basic objects
    private UserInput execute; //Executes certain actions in place of DisplayGUI
    private Controller controller; //To send information back to the controller
    private String extraInformation; //Extra information to display at the bottom

    //Main structure of the frame
    private JFrame frame;
    private JMenuBar menuBar;
    private JMenu gameMenu;
    private JMenu helpMenu;
    private JMenu settingsMenu;
    private JMenu difficultyMenu;
    private JMenu resolutionMenu;
    private JMenu backgroundMenu;

    private JMenuItem restartGameMenuItem;
    private JMenuItem quitGameMenuItem;
    private JMenuItem forceQuitGameMenuItem;

    private JMenuItem howToPlayMenuItem;
    private JMenuItem programInfoMenuItem;

    private JMenuItem normalDifficultyMenuItem;
    private JMenuItem easyDifficultyMenuItem;
    private JMenuItem veryEasyDifficultyItem;

    private JMenuItem safeResolutionMenuItem;
    private JMenuItem mediumResolutionMenuItem;
    private JMenuItem fullResolutionMenuItem;

    private JMenuItem standardBackgroundMenuItem;
    private JMenuItem alternative1BackgroundMenuItem;
    private JMenuItem alternative2BackgroundMenuItem;
    private JMenuItem alternative3BackgroundMenuItem;


    //Columns in the tableau
    private JPanel cardColumnPanel1;
    private JPanel cardColumnPanel2;
    private JPanel cardColumnPanel3;
    private JPanel cardColumnPanel4;
    private JPanel cardColumnPanel5;
    private JPanel cardColumnPanel6;
    private JPanel cardColumnPanel7;
    private JPanel cardColumnPanel8;

    //Free cells
    private JLabel freeCellLabel1;
    private JLabel freeCellLabel2;
    private JLabel freeCellLabel3;
    private JLabel freeCellLabel4;

    //Foundations
    private JLabel foundationLabel1;
    private JLabel foundationLabel2;
    private JLabel foundationLabel3;
    private JLabel foundationLabel4;

    //Information
    private JLabel extraInfo;

    //Code duplication removing lists
    ArrayList<JLabel> freeCells;
    ArrayList<JLabel> foundations;
    ArrayList<JPanel> cardColumns;

    //Time keeping information
    private double lastMoveSeconds;
    private double currentSeconds;

    //Keeps track of whether or not the card columns need to be reset
    private int[] prevCardColumnSizes;

    //Settings Display
    private int difficultySetting;
    private int backgroundSetting;
    private int resolutionSetting;

    /**
     * Constructor for the DisplayGUI
     *
     * @param controller The controller to notify of user input
     */
    public DisplayGUI(Controller controller)
    {
        this.controller = controller;
        execute = new UserInput();
        difficultySetting = controller.readConfig("Difficulty");
        resolutionSetting = controller.readConfig("Resolution");
        backgroundSetting = controller.readConfig("Background");
        freeCells = new ArrayList<>();
        foundations = new ArrayList<>();
        cardColumns = new ArrayList<>();
        extraInformation = "N/A";
        lastMoveSeconds = 0;
        currentSeconds = 0;
        prevCardColumnSizes = new int[8];

        makeFrame();
        for (int val : prevCardColumnSizes)
        {
            val = -1;
        }
    }

    /**
     * Disposes the frame
     */
    public void trashDisplay()
    {
        frame.dispose();
    }

    /**
     * Displays the images of the cards to the graphical table and displays a graphical placeholder otherwise.
     * It loads the images to display from the given file paths
     * https://stackoverflow.com/questions/14735085/clicking-a-jlabel-to-open-a-new-frame - Add a  mouse click action to labels
     * @param cardColumnImages The images of the cards for the card column
     * @param freeCellImages The images of the cards for the free cells
     * @param foundationImages The images of the cards for the foundation
     */
    public void updateTable(String[][] cardColumnImages, String[] freeCellImages, String[] foundationImages)
    {
        boolean[] cardColumnNeedReset = new boolean[8];
        for (JLabel freeCell : freeCells) //Remove the previous card images for reloading
        {
            clearCardCollection(freeCell);
        }
        for (JLabel foundation : foundations)
        {
            clearCardCollection(foundation);
        }
        int currentColumn = 0;
        for (JPanel cardColumn : cardColumns)
        {

            if (cardColumnImages[currentColumn].length == prevCardColumnSizes[currentColumn])
            {
                cardColumnNeedReset[currentColumn] = false;

            }
            else
            {
                clearCardCollection(cardColumn);
                cardColumnNeedReset[currentColumn] = true;
            }
            currentColumn++;
        }

        JLabel toAddLabel = null;
        for (int j = 0; j < cardColumns.size(); j++) //Add card images to the card columns first
        {
            if (cardColumnNeedReset[j])
            {
                final int innerValue = j; //Used to pass to the inner class
                if (cardColumnImages[j].length == 0)
                {
                    toAddLabel = new JLabel(new ImageIcon(fixCardSize("/card_column_empty.png")));
                    toAddLabel.addMouseListener(new MouseAdapter()
                    {

                        public void mouseClicked(MouseEvent e)
                        {
                            execute.selectCardColumn(innerValue + 1);
                        }
                    });
                    cardColumns.get(j).add(toAddLabel);
                } else
                {
                    for (int i = 0; i < cardColumnImages[j].length; i++)
                    {
                        toAddLabel = new JLabel(new ImageIcon(fixCardSize(cardColumnImages[j][i])));
                        if (i == cardColumnImages[j].length - 1)
                        {
                            toAddLabel.addMouseListener(new MouseAdapter()
                            {

                                public void mouseClicked(MouseEvent e)
                                {
                                    execute.selectCardColumn(innerValue + 1);
                                }
                            });
                        }
                        cardColumns.get(j).add(toAddLabel);
                    }
                }
                prevCardColumnSizes[j] = cardColumnImages[j].length;
            }
        }

        for (int i = 0; i < 4; i++)
        {
            final int innerValue = i;
            if (freeCellImages[i] == null)
            {
                freeCells.get(i).setIcon(new ImageIcon(fixCardSize("/freecell_empty.png")));
            }
            else
            {
                freeCells.get(i).setIcon(new ImageIcon(fixCardSize(freeCellImages[i])));
            }
            freeCells.get(i).addMouseListener(new MouseAdapter()   {

                public void mouseClicked(MouseEvent e)
                {
                    execute.selectFreeCell(innerValue+1);
                }
            });

            if (foundationImages[i] == null)
            {
                switch (i)
                {
                    case 0:
                        foundations.get(i).setIcon(new ImageIcon(fixCardSize("/foundation_hearts_empty.png")));
                        break;
                    case 1:
                        foundations.get(i).setIcon(new ImageIcon(fixCardSize("/foundation_diamonds_empty.png")));
                        break;
                    case 2:
                        foundations.get(i).setIcon(new ImageIcon(fixCardSize("/foundation_clubs_empty.png")));
                        break;
                    default:
                        foundations.get(i).setIcon(new ImageIcon(fixCardSize("/foundation_spades_empty.png")));
                        break;
                }
            }
            else
            {
                foundations.get(i).setIcon(new ImageIcon(fixCardSize(foundationImages[i])));
            }
            foundations.get(i).addMouseListener(new MouseAdapter()   {

                public void mouseClicked(MouseEvent e)
                {
                    execute.selectFoundation(innerValue+1);
                }
            });

        }
    }

    /**
     * Displays the extra information that a user may want while playing the game.
     *
     * https://stackoverflow.com/questions/6118922/convert-seconds-value-to-hours-minutes-seconds/6118983 - String.format that I wanted
     * @param timePlayed The time in seconds that the game has been played (starts after dealing out the cards)
     * @param turnsPlayed The number of turns that have been played
     * @param extraInformation A nullable field on what to set the extra information text to.
     */
    public void updateExtraInformation(double timePlayed, int turnsPlayed, @Nullable String extraInformation)
    {
        if (extraInformation != null)
        {
            this.extraInformation = extraInformation;
        }
        final String displayGUIExtraInformation = this.extraInformation;
        SwingWorker sw = new SwingWorker()
        {
            @Override
            protected Object doInBackground() throws Exception
            {
                int hours = (int) (timePlayed / 3600);
                int minutes = (int) ((timePlayed % 3600) / 60);
                int seconds = (int) (timePlayed % 60);
                currentSeconds = timePlayed;

                String formattedTime = String.format("%02d:%02d:%02d", hours, minutes, seconds);
                extraInfo.setText("Time: " + formattedTime + ", Moves Made: " + turnsPlayed + ", Information: " + displayGUIExtraInformation);
               return null;
            }
        };
        sw.execute();
        this.extraInformation = displayGUIExtraInformation;
    }

    /**
     * Shows a dialog letting them know why the move failed, it can be disabled
     * @param message The message to present the user if the move failed
     */
    public void moveFailed(String message)
    {
        JOptionPane.showMessageDialog(frame, message, "The move failed", JOptionPane.WARNING_MESSAGE);
    }

    /**
     * Shows a modal dialog asking the player if they wish to play again and congratulates them on their victory
     */
    public void gameWon()
    {
        if (JOptionPane.showConfirmDialog(frame, "Good job! You won this round, care for another?", "You won this round", JOptionPane.YES_NO_OPTION) == JOptionPane.YES_OPTION)
        {
            execute.restartGame();
        }
        else
        {
            execute.quitGame();
        }
    }

    /**
     * Good bye message given to the player before they exit out of the game
     */
    public void gameEnded()
    {
        JOptionPane.showMessageDialog(frame, "Thanks for playing, have a good day!");
    }

    /*
    Creates the frame and components and assembles them
     */
    private void makeFrame()
    {
        Color backgroundColor;
        switch (backgroundSetting) //This is the background for the extra information at the bottom
        {
            case 0:
                backgroundColor = new Color(145, 182, 183);
                break;
            case 1:
                backgroundColor = new Color(200,200,175);
                break;
            case 2:
                backgroundColor = new Color(69, 191, 107);
                break;
            default:
                backgroundColor = new Color(160, 129, 154);
                break;

        }

       frame = new JFrame("FreeCell Solitaire");
       frame.setIconImage(fixCardSize("/suits/Diamonds.png"));

       switch (resolutionSetting)
       {
           case 0:
               frame.setMinimumSize(new Dimension(1080, 720));
               break;
           case 1:
               frame.setMinimumSize(new Dimension(1350, 900));
               break;
           default:
               frame.setMinimumSize(new Dimension(1620, 980));
               break;
       }

       makeMenuBar();
       JPanel pane = new JPanel(new BorderLayout());
       frame.setContentPane(pane);
       pane.setBackground(backgroundColor);

       //-----Top-----
       JPanel topPanal = new JPanel(new FlowLayout(FlowLayout.CENTER, 24, 5));

       //---Free Cells---
       JPanel freeCellPanel = new JPanel(new GridLayout(1, 4, 8, 2));
       freeCellLabel1 = new JLabel(new ImageIcon(fixCardSize("/freecell_empty.png")));
       freeCellLabel2 = new JLabel(new ImageIcon(fixCardSize("/freecell_empty.png")));
       freeCellLabel3 = new JLabel(new ImageIcon(fixCardSize("/freecell_empty.png")));
       freeCellLabel4 = new JLabel(new ImageIcon(fixCardSize("/freecell_empty.png")));

       freeCells.add(freeCellLabel1);
       freeCells.add(freeCellLabel2);
       freeCells.add(freeCellLabel3);
       freeCells.add(freeCellLabel4);
        for (JLabel freeCell : freeCells)
        {
            freeCellPanel.add(freeCell);
        }
       topPanal.add(freeCellPanel);

       //---Foundations---
        JPanel foundationPanel = new JPanel(new GridLayout(1, 4, 8, 2));
        foundationLabel1 = new JLabel(new ImageIcon(fixCardSize("/foundation_hearts_empty.png")));
        foundationLabel2 = new JLabel(new ImageIcon(fixCardSize("/foundation_diamonds_empty.png")));
        foundationLabel3 = new JLabel(new ImageIcon(fixCardSize("/foundation_clubs_empty.png")));
        foundationLabel4 = new JLabel(new ImageIcon(fixCardSize("/foundation_spades_empty.png")));

        foundations.add(foundationLabel1);
        foundations.add(foundationLabel2);
        foundations.add(foundationLabel3);
        foundations.add(foundationLabel4);
        for (JLabel foundation : foundations)
        {
            foundationPanel.add(foundation);
        }
        topPanal.add(foundationPanel);

       //-----Center-----
        JPanel centerPanel = new JPanel(new FlowLayout());
        JPanel innerCenterPanel = new JPanel();
        innerCenterPanel.setLayout(new GridLayout(1, 8, 10, 0));

        cardColumnPanel1 = new JPanel();
        cardColumnPanel2 = new JPanel();
        cardColumnPanel3 = new JPanel();
        cardColumnPanel4 = new JPanel();
        cardColumnPanel5 = new JPanel();
        cardColumnPanel6 = new JPanel();
        cardColumnPanel7 = new JPanel();
        cardColumnPanel8 = new JPanel();

        cardColumns.add(cardColumnPanel1);
        cardColumns.add(cardColumnPanel2);
        cardColumns.add(cardColumnPanel3);
        cardColumns.add(cardColumnPanel4);
        cardColumns.add(cardColumnPanel5);
        cardColumns.add(cardColumnPanel6);
        cardColumns.add(cardColumnPanel7);
        cardColumns.add(cardColumnPanel8);
        for (JPanel cardColumn : cardColumns)
        {
            cardColumn.setLayout(new OverlapLayout(new Point(0, 30)));
            innerCenterPanel.add(cardColumn);
        }
        centerPanel.add(innerCenterPanel);

       //-----Bottom-----
       extraInfo = new JLabel("Time: N/A, Moves Made: N/A, Information: N/A");
       extraInfo.setForeground(new Color(245, 245, 245));
        switch (resolutionSetting)
        {
            case 0:
                extraInfo.setFont(new Font("Test", Font.BOLD, 20));
                break;
            case 1:
                extraInfo.setFont(new Font("Test", Font.BOLD, 25));
                break;
            default:
                extraInfo.setFont(new Font("Test", Font.BOLD, 30));
                break;

        }


       pane.add(topPanal, BorderLayout.NORTH);
       pane.add(centerPanel, BorderLayout.CENTER);
       pane.add(extraInfo, BorderLayout.SOUTH);

        frame.setDefaultCloseOperation(JFrame.DO_NOTHING_ON_CLOSE);
        frame.addWindowListener(new WindowAdapter() { //https://stackoverflow.com/questions/16372241/run-function-on-jframe-close
            @Override
            public void windowClosing(WindowEvent event) {
                execute.quitGame();
            }
        });
       fixBackgroundPane(pane);
       frame.setLocationRelativeTo(null);
       frame.pack();
       frame.setVisible(true);
    }

    /*
    Creates the menu bar
     */
    private void makeMenuBar()
    {
        Color backgroundColor = new Color(239, 45, 86);
        Color foregroundColor = new Color(240, 240, 240);
        menuBar = new JMenuBar();

        gameMenu = new JMenu("Game");
        gameMenu.setBackground(backgroundColor);
        gameMenu.setForeground(foregroundColor);
        restartGameMenuItem = new JMenuItem("Reset Game");
        restartGameMenuItem.addActionListener(a -> execute.restartGame());
        restartGameMenuItem.setAccelerator(KeyStroke.getKeyStroke('R', KeyEvent.CTRL_DOWN_MASK));
        restartGameMenuItem.setBackground(backgroundColor);
        restartGameMenuItem.setForeground(foregroundColor);
        quitGameMenuItem = new JMenuItem("Quit Game");
        quitGameMenuItem.addActionListener(a -> execute.quitGame());
        quitGameMenuItem.setAccelerator(KeyStroke.getKeyStroke('Q', KeyEvent.CTRL_DOWN_MASK));
        quitGameMenuItem.setBackground(backgroundColor);
        quitGameMenuItem.setForeground(foregroundColor);
        forceQuitGameMenuItem = new JMenuItem("Force Quit");
        forceQuitGameMenuItem.addActionListener(a -> execute.forceQuitGame());
        forceQuitGameMenuItem.setAccelerator(KeyStroke.getKeyStroke('F', KeyEvent.CTRL_DOWN_MASK));
        forceQuitGameMenuItem.setBackground(backgroundColor);
        forceQuitGameMenuItem.setForeground(foregroundColor);

        helpMenu = new JMenu("Help");
        helpMenu.setBackground(backgroundColor);
        helpMenu.setForeground(foregroundColor);
        howToPlayMenuItem = new JMenuItem("How to Play");
        howToPlayMenuItem.addActionListener(a -> execute.showHowToPlayDialog());
        howToPlayMenuItem.setAccelerator(KeyStroke.getKeyStroke('H', KeyEvent.CTRL_DOWN_MASK));
        howToPlayMenuItem.setBackground(backgroundColor);
        howToPlayMenuItem.setForeground(foregroundColor);
        programInfoMenuItem = new JMenuItem("Program Info");
        programInfoMenuItem.addActionListener(a -> execute.showProgramInfo());
        programInfoMenuItem.setAccelerator(KeyStroke.getKeyStroke('I', KeyEvent.CTRL_DOWN_MASK));
        programInfoMenuItem.setBackground(backgroundColor);
        programInfoMenuItem.setForeground(foregroundColor);

        settingsMenu = new JMenu("Settings");
        settingsMenu.setBackground(new Color(239, 45, 86));
        settingsMenu.setForeground(foregroundColor);
        difficultyMenu = new JMenu("Difficulty");
        difficultyMenu.setBackground(new Color(239, 45, 86));
        difficultyMenu.setForeground(new Color(239, 45, 86));
        resolutionMenu = new JMenu("Resolution");
        resolutionMenu.setBackground(new Color(239, 45, 86));
        resolutionMenu.setForeground(new Color(239, 45, 86));
        backgroundMenu = new JMenu("Background");
        backgroundMenu.setBackground(new Color(239, 45, 86));
        backgroundMenu.setForeground(new Color(239, 45, 86));

        normalDifficultyMenuItem = new JMenuItem("Normal");
        normalDifficultyMenuItem.addActionListener(a -> execute.changeDifficulty(0));
        normalDifficultyMenuItem.setBackground(backgroundColor);
        normalDifficultyMenuItem.setForeground(foregroundColor);
        easyDifficultyMenuItem = new JMenuItem("Easy");
        easyDifficultyMenuItem.addActionListener(a -> execute.changeDifficulty(1));
        easyDifficultyMenuItem.setBackground(backgroundColor);
        easyDifficultyMenuItem.setForeground(foregroundColor);
        veryEasyDifficultyItem = new JMenuItem("Very Easy");
        veryEasyDifficultyItem.addActionListener(a -> execute.changeDifficulty(2));
        veryEasyDifficultyItem.setBackground(backgroundColor);
        veryEasyDifficultyItem.setForeground(foregroundColor);

        safeResolutionMenuItem = new JMenuItem("Small 1080x720");
        safeResolutionMenuItem.addActionListener(a -> execute.changeResolution(0));
        safeResolutionMenuItem.setBackground(backgroundColor);
        safeResolutionMenuItem.setForeground(foregroundColor);
        mediumResolutionMenuItem = new JMenuItem("Medium 1350x900");
        mediumResolutionMenuItem.addActionListener(a -> execute.changeResolution(1));
        mediumResolutionMenuItem.setBackground(backgroundColor);
        mediumResolutionMenuItem.setForeground(foregroundColor);
        fullResolutionMenuItem = new JMenuItem("Large 1620x1080");
        fullResolutionMenuItem.addActionListener(a -> execute.changeResolution(2));
        fullResolutionMenuItem.setBackground(backgroundColor);
        fullResolutionMenuItem.setForeground(foregroundColor);

        standardBackgroundMenuItem = new JMenuItem("Default Powder Blue");
        standardBackgroundMenuItem.addActionListener(a -> execute.changeBackground(0));
        standardBackgroundMenuItem.setBackground(backgroundColor);
        standardBackgroundMenuItem.setForeground(foregroundColor);
        alternative1BackgroundMenuItem = new JMenuItem("Beige");
        alternative1BackgroundMenuItem.addActionListener(a -> execute.changeBackground(1));
        alternative1BackgroundMenuItem.setBackground(backgroundColor);
        alternative1BackgroundMenuItem.setForeground(foregroundColor);
        alternative2BackgroundMenuItem = new JMenuItem("Slate Grey");
        alternative2BackgroundMenuItem.addActionListener(a -> execute.changeBackground(2));
        alternative2BackgroundMenuItem.setBackground(backgroundColor);
        alternative2BackgroundMenuItem.setForeground(foregroundColor);
        alternative3BackgroundMenuItem = new JMenuItem("Pale Violet");
        alternative3BackgroundMenuItem.addActionListener(a -> execute.changeBackground(3));
        alternative3BackgroundMenuItem.setBackground(backgroundColor);
        alternative3BackgroundMenuItem.setForeground(foregroundColor);

        gameMenu.add(restartGameMenuItem);
        gameMenu.add(quitGameMenuItem);
        gameMenu.add(forceQuitGameMenuItem);

        helpMenu.add(howToPlayMenuItem);
        helpMenu.add(programInfoMenuItem);

        difficultyMenu.add(normalDifficultyMenuItem);
        difficultyMenu.add(easyDifficultyMenuItem);
        difficultyMenu.add(veryEasyDifficultyItem);
        resolutionMenu.add(safeResolutionMenuItem);
        resolutionMenu.add(mediumResolutionMenuItem);
        resolutionMenu.add(fullResolutionMenuItem);
        backgroundMenu.add(standardBackgroundMenuItem);
        backgroundMenu.add(alternative1BackgroundMenuItem);
        backgroundMenu.add(alternative2BackgroundMenuItem);
        backgroundMenu.add(alternative3BackgroundMenuItem);
        settingsMenu.add(difficultyMenu);
        settingsMenu.add(resolutionMenu);
        settingsMenu.add(backgroundMenu);


        menuBar.add(gameMenu);
        menuBar.add(helpMenu);
        menuBar.add(settingsMenu);

        disableAppropriateMenus();
        menuBar.setOpaque(true);
        menuBar.setBackground(new Color(239, 45, 86));
        frame.setJMenuBar(menuBar);
    }

    /*
    Recolors the backgrounds of components to be that nice blue color
    https://stackoverflow.com/questions/27774581/change-background-color-of-components-with-reference-to-color-variable-java
     */
    private void fixBackgroundPane(Container currentComponent)
    {
        Color backgroundColor;
        switch (backgroundSetting)
        {
            case 0:
                backgroundColor = new Color(160, 197, 196);
                break;
            case 1:
                backgroundColor = new Color(215,215,190);
                break;
            case 2:
                backgroundColor = new Color(84, 106, 123);
                break;
            default:
                backgroundColor = new Color(175, 144, 169);
                break;

        }
        for(Component c : currentComponent.getComponents())
        {
            if(c instanceof Container)
            {
                if(c instanceof JPanel)
                {
                    c.setBackground(backgroundColor);
                }

                fixBackgroundPane((Container)c);
            }
        }
    }

    /*
    Takes in a file path to a image and returns that card's image at the correct size
    https://stackoverflow.com/questions/6714045/how-to-resize-jlabel-imageicon
    https://stackoverflow.com/questions/3861989/preferred-way-of-loading-resources-in-java
     */
    private Image fixCardSize(String filepath)
    {
        ImageIcon icon = new ImageIcon(getClass().getResource(filepath));
        Image image = icon.getImage();
        switch (resolutionSetting)
        {
            case 0:
                return image.getScaledInstance(83, 120,  java.awt.Image.SCALE_SMOOTH);
            case 1:
                return image.getScaledInstance(104, 150,  java.awt.Image.SCALE_SMOOTH);
            default:
                return image.getScaledInstance(125, 180,  java.awt.Image.SCALE_SMOOTH);

        }

    }

    /*
    Clears the JLabels from a card collection and deallocates the memory used by them.
     */
    private void clearCardCollection(JComponent cardCollection)
    {
        for (Component label : cardCollection.getComponents())
        {
            ((JLabel) label).setIcon(null);
            label.setVisible(false);
            MouseListener ml = (label.getMouseListeners().length > 0) ? label.getMouseListeners()[0] : null;
            if (ml != null)
            {
                label.removeMouseListener(ml);
            }
            label = null;
        }
        cardCollection.removeAll();
    }

    /*
        Toggles the menus as needed for the settings
         */
    private void disableAppropriateMenus()
    {
        switch (difficultySetting)
        {
            case 0:
                normalDifficultyMenuItem.setEnabled(false);
                easyDifficultyMenuItem.setEnabled(true);
                veryEasyDifficultyItem.setEnabled(true);
                break;
            case 1:
                normalDifficultyMenuItem.setEnabled(true);
                easyDifficultyMenuItem.setEnabled(false);
                veryEasyDifficultyItem.setEnabled(true);
                break;
            case 2:
                normalDifficultyMenuItem.setEnabled(true);
                easyDifficultyMenuItem.setEnabled(true);
                veryEasyDifficultyItem.setEnabled(false);
                break;
        }

        switch (resolutionSetting)
        {
            case 0:
                safeResolutionMenuItem.setEnabled(false);
                mediumResolutionMenuItem.setEnabled(true);
                fullResolutionMenuItem.setEnabled(true);
                break;
            case 1:
                safeResolutionMenuItem.setEnabled(true);
                mediumResolutionMenuItem.setEnabled(false);
                fullResolutionMenuItem.setEnabled(true);
                break;
            case 2:
                safeResolutionMenuItem.setEnabled(true);
                mediumResolutionMenuItem.setEnabled(true);
                fullResolutionMenuItem.setEnabled(false);
                break;
        }

        switch (backgroundSetting)
        {
            case 0:
                standardBackgroundMenuItem.setEnabled(false);
                alternative1BackgroundMenuItem.setEnabled(true);
                alternative2BackgroundMenuItem.setEnabled(true);
                alternative3BackgroundMenuItem.setEnabled(true);
                break;
            case 1:
                standardBackgroundMenuItem.setEnabled(true);
                alternative1BackgroundMenuItem.setEnabled(false);
                alternative2BackgroundMenuItem.setEnabled(true);
                alternative3BackgroundMenuItem.setEnabled(true);
                break;
            case 2:
                standardBackgroundMenuItem.setEnabled(true);
                alternative1BackgroundMenuItem.setEnabled(true);
                alternative2BackgroundMenuItem.setEnabled(false);
                alternative3BackgroundMenuItem.setEnabled(true);
                break;
            default:
                standardBackgroundMenuItem.setEnabled(true);
                alternative1BackgroundMenuItem.setEnabled(true);
                alternative2BackgroundMenuItem.setEnabled(true);
                alternative3BackgroundMenuItem.setEnabled(false);
                break;
        }
    }



    /*-------------------------------
    The user input is handled by this inner class. It is directed here by action listeners
     */
    private class UserInput
    {
        /*
        Creates the user input and allows it to work with parts of the display
         */
        private UserInput()
        {

        }

        /*
        Selects a foundation and notifies the controller
         */
        private void selectFoundation(int option)
        {
            if (!canSelectCard())
            {
                return;
            }
            controller.selectCard(4+option);
            lastMoveSeconds = currentSeconds;
        }

        /*
        Selects a free cell and notifies the controller
         */
        private void selectFreeCell(int option)
        {
            if (!canSelectCard())
            {
                return;
            }
            controller.selectCard(option);
            lastMoveSeconds = currentSeconds;
        }

        /*
        Selects a card column and notifies the controller
         */
        private void selectCardColumn(int option)
        {
            if (!canSelectCard())
            {
                return;
            }
            controller.selectCard(8+option);
            lastMoveSeconds = currentSeconds;
        }

        /*
        Shows the dialog on how to play
         */
        private void showHowToPlayDialog()
        {
            JDialog jDialog = new JDialog(frame, "How To Play", false);
            JTextArea content = new JTextArea(30, 50);
            content.setFont(new Font("Readable", Font.BOLD, 16));
            content.append(" In FreeCell Solitaire there are free cells depicted on the top left, \n foundations on the top right, and 8 card columns.\n");
            content.append(" The free cells allow a single card to be stored in them at any point.\n");
            content.append(" The foundations allow cards of their suit to be added in ascending order\n starting from aces and going to kings.\n");
            content.append(" The card columns allow any card when empty but must be added in a \n descending order with the alternating suit color each time afterward.\n");
            content.append(" Cards must be moved from the bottom of the card columns represented \n on the screen, but are technically on the top of the card column.\n\n");
            content.append(" All cards from the card column can be seen at once, but only the top \n card can be seen in the foundations.\n\n");
            content.append(" The game starts by dealing one card per column and then doing subsequent \n rows past the first until there are no cards left to deal from the 52 cards in a no joker deck.\n");
            content.append(" The game is won when all foundations have thirteen cards\n\n\n");
            content.append(" Click the X on the top right of this dialog to close it \n or click to go to a web link with a better rundown.\n");
            content.append(" Nothing will happen on mouse click if access to the default\n web browser is disallowed or the link was not found");
            content.addMouseListener(new MouseAdapter()
            {
                public void mouseClicked(MouseEvent me)
                {
                    showWebLink();
                }
            });
            setUpDialogAndShow(jDialog, content);
        }

        /*
        Shows information related to the program
         */
        private void showProgramInfo()
        {
            JDialog jDialog = new JDialog(frame, "Program Info", false);
            JTextArea content = new JTextArea(30, 50);
            content.setFont(new Font("Readable", Font.BOLD, 16));
            content.append(" Created by Devon X. Dalrymple\n");
            content.append(" Version 1.0.0 | Settings set apply when the game is reset\n\n");
            content.append(" This project was aided in its creation by these creators:\n\n");
            content.append(" https://code.google.com/archive/p/vector-playing-cards/ \n Provides the playing cards used in this game\n [Byron Knoll]\n\n");
            content.append(" https://pixabay.com/vectors/playing-cards-cards-suit-spades-2091948/ \n Provides images for the four suits\n [User: PixLoger]\n\n");
            content.append(" https://tips4java.wordpress.com/2009/07/26/overlap-layout/ \n Provides a layout to handle the z-indexing of the card columns\n [Rob Camick]\n\n\n");
            content.append(" Click the X on the top right of this dialog to close it.");
            content.setEditable(false);
            setUpDialogAndShow(jDialog, content);
        }

        /*
        Show web link
         */
        private void showWebLink()
        {
            try
            {
                Desktop.getDesktop().browse(new URI("https://semicolon.com/Solitaire/Rules/FreeCell.html"));
            }
            catch (Exception ignored)
            {
            }

        }

        /*
        Sets up display options for the dialog and shows it
         */
        private void setUpDialogAndShow(JDialog jDialog, Container content)
        {
            jDialog.setSize(720, 520);
            jDialog.setLocationRelativeTo(null);
            jDialog.setContentPane(content);
            jDialog.setVisible(true);
        }

        /*
        Tells the controller to reset the game
         */
        private void restartGame()
        {
            controller.restartGame();
        }

        /*
        Tells the controller to quit out of the game and displays a goodbye message to the user
         */
        private void quitGame()
        {
            controller.endGame();
            gameEnded();
            frame.dispose();
            System.exit(0);
        }

        /*
        Immediately shuts down the program, very harsh and does not allow cleanup
         */
        private void forceQuitGame()
        {
            System.exit(-1);
        }

        /*
        Disallows instant-moves to allow time to deselect cards and prevent bugs
         */
        private boolean canSelectCard()
        {
            return (currentSeconds > (lastMoveSeconds + .15));
        }

        /*
        Changes the difficulty setting
         */
        private void changeDifficulty(int option)
        {
            HashMap<String, Integer> hashMap = new HashMap<>();
            hashMap.put("Difficulty", option);

            if (controller.alterConfig(hashMap))
            {
                switch (option)
                {
                    case 0:
                        normalDifficultyMenuItem.setEnabled(false);
                        easyDifficultyMenuItem.setEnabled(true);
                        veryEasyDifficultyItem.setEnabled(true);
                        break;
                    case 1:
                        normalDifficultyMenuItem.setEnabled(true);
                        easyDifficultyMenuItem.setEnabled(false);
                        veryEasyDifficultyItem.setEnabled(true);
                        break;
                    case 2:
                        normalDifficultyMenuItem.setEnabled(true);
                        easyDifficultyMenuItem.setEnabled(true);
                        veryEasyDifficultyItem.setEnabled(false);
                        break;
                }
            }
        }

        /*
        Changes the difficulty setting
         */
        private void changeResolution(int option)
        {
            HashMap<String, Integer> hashMap = new HashMap<>();
            hashMap.put("Resolution", option);

            if (controller.alterConfig(hashMap))
            {
                switch (option)
                {
                    case 0:
                        safeResolutionMenuItem.setEnabled(false);
                        mediumResolutionMenuItem.setEnabled(true);
                        fullResolutionMenuItem.setEnabled(true);
                        break;
                    case 1:
                        safeResolutionMenuItem.setEnabled(true);
                        mediumResolutionMenuItem.setEnabled(false);
                        fullResolutionMenuItem.setEnabled(true);
                        break;
                    case 2:
                        safeResolutionMenuItem.setEnabled(true);
                        mediumResolutionMenuItem.setEnabled(true);
                        fullResolutionMenuItem.setEnabled(false);
                        break;
                }
            }
        }

        /*
        Changes the difficulty setting
         */
        private void changeBackground(int option)
        {
            HashMap<String, Integer> hashMap = new HashMap<>();
            hashMap.put("Background", option);

            if (controller.alterConfig(hashMap))
            {
                switch (option)
                {
                    case 0:
                        standardBackgroundMenuItem.setEnabled(false);
                        alternative1BackgroundMenuItem.setEnabled(true);
                        alternative2BackgroundMenuItem.setEnabled(true);
                        alternative3BackgroundMenuItem.setEnabled(true);
                        break;
                    case 1:
                        standardBackgroundMenuItem.setEnabled(true);
                        alternative1BackgroundMenuItem.setEnabled(false);
                        alternative2BackgroundMenuItem.setEnabled(true);
                        alternative3BackgroundMenuItem.setEnabled(true);
                        break;
                    case 2:
                        standardBackgroundMenuItem.setEnabled(true);
                        alternative1BackgroundMenuItem.setEnabled(true);
                        alternative2BackgroundMenuItem.setEnabled(false);
                        alternative3BackgroundMenuItem.setEnabled(true);
                        break;
                    default:
                        standardBackgroundMenuItem.setEnabled(true);
                        alternative1BackgroundMenuItem.setEnabled(true);
                        alternative2BackgroundMenuItem.setEnabled(true);
                        alternative3BackgroundMenuItem.setEnabled(false);
                        break;
                }
            }
        }
    }
}
