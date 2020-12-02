# Common workflow
## Copy project folder "My Extensions" & move it to the ""..\ANSYS Inc.\My Extensions"

## Build "Ansys" project
## Move "Ansys.dll" library to "..\ACT\bin\x64\" folder

## Open Ansys 17.0
## Select "ACT Start Page" tab"
## Click on "ACT Console" button
## Write code below:
clr.AddReference("Ansys")
from Ansys import *
gena = GenA(ExtAPI)
