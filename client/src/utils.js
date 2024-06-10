function formatDate(date) {
    const year = date.getFullYear();
    const month = ('0' + (date.getMonth() + 1)).slice(-2);
    const day = ('0' + date.getDate()).slice(-2);
    return `${year}-${month}-${day}`;
}

function daysOfTwoDate(date1, date2) {
    // 获取两个日期对象的时间戳
    let time1 = new Date(date1).getTime();
    let time2 = new Date(date2).getTime();
    // 计算相差的毫秒数
    let diff = Math.abs(time1 - time2);
    // 将毫秒数转换为天数
    let days = Math.floor(diff / (1000 * 60 * 60 * 24)) + 1;
    return days
}

export {formatDate, daysOfTwoDate}