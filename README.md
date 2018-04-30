# SpydazWebAI_Service
This is a windows service with a Wrapper to control the service. Custom-Commands can be sent to the service for it to execute; The Service returns information via TCP/UDP. ; To implment the service will need to be running (create installer) And A refference to the AI_ServiceController made; The controller controls the service


This service is very basic and yet designed to hold the Artificial intelligence STATE (emotions) .... Currently The communications are very sketchy! The internal Emotion Engine is Currently under design; Yet it will be based in various timers / and maybe Biorhythms. 
# **THE EMOTION ENGINE**

Small Internal Timers Implemented When changing Emotion Emotion is held for set time before returning to neutral, If the same emotion is sent then its timer is extended; If the new emotion is detected Time is deducted;

Eventually Each Emotion will have a numerical value..... The value sent to the control for the control to identify the emotion; the value sent are to be decided by the calling script! 

# **Therfore** 
As positive values can be added to negative values;
If negative emotions have negative values and positive emotions have positive values; 
# **Then** 
By adding the current emotion to the new emotion will produce a new current emotion;
The value produced would need to be matched to the closest recognised emotion; 

Values without labels may be taken to be unknown/unlabelled emotions given a list of Ranked but un-valued emotions identification tags;
a probable match may be determined by locating an estimated tag between known values.  


# **As simple as it can get** 

**Angry = -1**

**sad**

**Neutral = 0**

**suprise**

**joy**

**Happy = +1**

**Current Emotion + New Emotion = New Current Emotion**

**-1 + 0.5 = 0.5 = Sad**

**1 + -0.3 = 0.6 Joy**

