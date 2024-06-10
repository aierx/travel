namespace webapi.model.po;

public class BaseResult
{
    public int code { set; get; } = 200;

    public Object value { set; get; }

    public BaseResult(int code, Object value)
    {
        this.code = code;
        this.value = value;
    }

    public static BaseResult Ok(Object value)
    {
        return new BaseResult(code: 200, value: value);
    }

    public static BaseResult Error(Object value)
    {
        return new BaseResult(code: 300, value: value);
    }
}