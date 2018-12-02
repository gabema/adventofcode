/*
http://adventofcode.com/2017/day/3
147  142  133  122   59
304    5    4    2   57
330   10    1    1   54
351   11   23   25   26
362  747  806  880  931
*/

const spot = 368078;

const calcSpotValue = (x, y, map) => {
    let sum = 0;
    sum += map['' + (x-1) + 'x' + (y - 1)] || 0;
    sum += map['' + (x-1) + 'x' + y] || 0;
    sum += map['' + (x-1) + 'x' + (y + 1)] || 0;
    sum += map['' + (x+1) + 'x' + (y - 1)] || 0;
    sum += map['' + (x+1) + 'x' + y] || 0;
    sum += map['' + (x+1) + 'x' + (y + 1)] || 0;
    sum += map['' + x + 'x' + (y - 1)] || 0;
    sum += map['' + x + 'x' + (y + 1)] || 0;
    return sum;
}

function *XYSpotIterator(foundValue) {
    let radius = 0;
    let spot = 0;
    let map = {
        "0x0": 1
    };

    while (radius < 6) {    
        let x, y;
        // right side
        x = radius + 1;
        for (y = 0 - radius; y < radius + 2; y++ ) {
            spot = calcSpotValue(x, y, map);
            map['' + x + 'x' + y] = spot;
            yield({ x, y, spot });
        }
        // top side (minus right corner)
        y = radius + 1;
        for (x = radius; x > 0 - radius - 2; x-- ) {
            spot = calcSpotValue(x, y, map);
            map['' + x + 'x' + y] = spot;
            yield({ x, y, spot });
        }
        // left side
        x = 0 - radius - 1;
        for (y = radius; y > 0 - radius - 1; y-- ) {
            spot = calcSpotValue(x, y, map);
            map['' + x + 'x' + y] = spot;
            yield({ x, y, spot });
        }
        // bottom side
        y = 0 - radius - 1;
        for (x = 0 - radius - 1; x < radius + 2; x++ ) {
            spot = calcSpotValue(x, y, map);
            map['' + x + 'x' + y] = spot;
            yield({ x, y, spot });
        }
        radius++
    }
}

const getXYForSpotLargerThan = spot => {
    for (let item of XYSpotIterator(spot)) {
        if (item.spot > spot) {
            return {
                x: item.x,
                y: item.y,
                spot: item.spot,
                NumJumps: Math.abs(item.x) + Math.abs(item.y)
            }
        }
    }
};

console.log(getXYForSpotLargerThan(spot));
