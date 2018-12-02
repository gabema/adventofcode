/*
http://adventofcode.com/2017/day/3
37 36  35  34  33  32 31
38 17  16  15  14  13 30
39 18   5   4   3  12 29
40 19   6   1   2  11 28
41 20   7   8   9  10 27
42 21  22  23  24  25 26
43 44  45  46  47  48 ==>
*/

const spot = 368078;

function *XYSpotIterator(dimension) {
    let radius = Math.floor(dimension / 2);
    let spot = dimension * dimension + 1;
    let x, y;
    // right side
    x = radius + 1;
    for (y = 0 - radius; y < radius + 2; y++ ) {
        yield({ x, y, spot });
        spot++;
    }
    // top side (minus right corner)
    y = radius + 1;
    for (x = radius; x > 0 - radius - 2; x-- ) {
        yield({ x, y, spot });
        spot++;
    }
    // left side
    x = 0 - radius - 1;
    for (y = radius; y > 0 - radius - 1; y-- ) {
        yield({ x, y, spot });
        spot++;
    }
    // bottom side
    y = 0 - radius - 1;
    for (x = 0 - radius - 1; x < radius + 2; x++ ) {
        yield({ x, y, spot });
        spot++;
    }
}

const getXYForSpot = spot => {
    let smallerDimension = Math.floor(Math.sqrt(spot - 1));
    if (smallerDimension % 2 === 0) {
        smallerDimension--
    }

    for (let item of XYSpotIterator(smallerDimension)) {
        if (item.spot === spot) {
            return {
                x: item.x,
                y: item.y,
                NumJumps: Math.abs(item.x) + Math.abs(item.y)
            }
        }
    }
};

console.log(getXYForSpot(spot));
