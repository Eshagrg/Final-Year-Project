
let currentRollNumber = 1;


function provideRollNumber(vacancy_id) {
    if (currentRollNumber <= NoOfPos) {
        const rollNumber = currentRollNumber;
        currentRollNumber++;
        return rollNumber;
    } else {
        return "No vacancy available.";
    }
}
for (let i = 0; i < NoOfPos; i++) {
    const rollNumber = provideRollNumber(vacancy_id);
    if (typeof rollNumber === "number") {
        console.log(`Assigned roll number: ${rollNumber}`);
    } else {
        console.log(rollNumber);
    }
}
